using System;
using System.Collections.Generic;
using System.Linq;
using Bs.PlanarGraph.Algorithm.Entities;

namespace Bs.PlanarGraph.Algorithm
{
    public class PlanarityTester
    {
        public Graph MainGraph { get; set; }
        public List<Graph> GraphParts { get; set; } // <- Components of main graph, where planarity of all components mean planarity of main graph
        public Graph SubGraph { get; set; }
        public PlanarityResult PlanarityResult { get; set; }
        public SimplifyResult SimplifyResult { get; set; }

        public PlanarityTester(Graph graph)
        {
            Graph graphPart = (Graph)graph.Clone();
            GraphParts = new List<Graph> {graphPart};
            PlanarityResult = PlanarityResult.Undetermined;
        }

        // Main simplifying function.
        // Should be called asynchronously to show status texts at each step.
        public IEnumerable<string> ApplySimplifyingStep()
        {
            SimplifyResult = SimplifyResult.CannotSimplifyAgain;

            List<Graph> newGraphParts = new List<Graph>();

            // PREPROCESSING 1: If the graph is not connected, then consider each component separately.
            foreach (Graph graphPart in GraphParts)
            {
                newGraphParts.AddRange(graphPart.SplitDisonnectedComponents());
            }
            if (newGraphParts.Count > GraphParts.Count)
            {
                // When no simplification is done in a simplify step, then it can be said
                // that the graph can no longer be simplified.
                // Here, we did a simplification and we may simplify again in the next step.
                SimplifyResult = SimplifyResult.CanSimplifyAgain;
                GraphParts = newGraphParts;
            }

            yield return "PREPROCESS STEP 1: Seperated disconnected components...";

            newGraphParts = new List<Graph>();

            // PREPROCESSING 2: If the graph has cut-vertices, then test each block separately.
            foreach (Graph graphPart in GraphParts)
            {
                CutVerticesFinder cvf = new CutVerticesFinder();
                List<Node> cutNodes = cvf.FindCutVertices(graphPart);
                if (cutNodes != null && cutNodes.Count > 1)
                {
                    SimplifyResult = SimplifyResult.CanSimplifyAgain;
                    // Since we consider "blocks" we need to add back the edges incident to
                    // cut nodes to get blocks including cut nodes and their edges.
                    List<Edge> edgesToAddBack = new List<Edge>();
                    foreach (Node cutNode in cutNodes)
                    {
                        List<Edge> edges = graphPart.GetEdges(cutNode);
                        edgesToAddBack.AddRange(edges);
                        // Cut nodes AND their edges are removed,
                        // they will be added back to corresponding blocks.
                        edges.ForEach(e => graphPart.Edges.Remove(e));
                        graphPart.Nodes.Remove(cutNode);
                    }
                    foreach (Graph disonnectedComponent in graphPart.SplitDisonnectedComponents())
                    {
                        // Now we add back the edges.
                        foreach (Edge edgeToAddBack in edgesToAddBack)
                        {
                            Node nodeInComponent = disonnectedComponent.Nodes.FirstOrDefault(n => n == edgeToAddBack.Node1 || n == edgeToAddBack.Node2);
                            if (nodeInComponent != null)
                            {
                                Node cutNode = edgeToAddBack.Node1 == nodeInComponent ? edgeToAddBack.Node2 : edgeToAddBack.Node1;
                                disonnectedComponent.AddNode(cutNode);
                                disonnectedComponent.AddEdge(edgeToAddBack);
                            }
                        }
                        newGraphParts.Add(disonnectedComponent);
                    }
                }
                else
                {
                    newGraphParts.Add(graphPart);
                }
            }
            GraphParts = newGraphParts;

            yield return "PREPROCESS STEP 2: If the graph has cut vertices, seperated all blocks of the graph...";

            // PREPROCESSING 3: Each vertex of degree 2 plus its incident edges can be replaced by a single edge.
            foreach (Graph graphPart in GraphParts)
            {
                for (int i = 0; i < graphPart.Nodes.Count; i++)
                {
                    List<Edge> edges = graphPart.GetEdges(graphPart.Nodes[i]);
                    if (edges.Count == 1)
                    {
                        // Node with degree 1 is irrelevant to planarity, we can just remove.
                        // This step may even be not needed (already removed previously).
                        graphPart.Edges.Remove(edges[0]);
                        graphPart.Nodes.Remove(graphPart.Nodes[i]);
                        i--;
                        SimplifyResult = SimplifyResult.CanSimplifyAgain;
                    }
                    if (edges.Count == 2)
                    {
                        // Node with degree 2, delete node and merge edges.
                        Node node1 = edges[0].Node1 == graphPart.Nodes[i] ? edges[0].Node2 : edges[0].Node1;
                        Node node2 = edges[1].Node1 == graphPart.Nodes[i] ? edges[1].Node2 : edges[1].Node1;
                        graphPart.Edges.Remove(edges[0]);
                        graphPart.Edges.Remove(edges[1]);
                        graphPart.Nodes.Remove(graphPart.Nodes[i]);
                        graphPart.GetOrCreateEdge(node1, node2);
                        i--;
                        SimplifyResult = SimplifyResult.CanSimplifyAgain;
                    }
                }
            }

            yield return "PREPROCESS STEP 3: Each vertex of degree 2 plus its incident edges is replaced by a single edge...";

            // Remove planar components from "GraphParts" or determine if any of components are non-planar
            List<Graph> partsToRemove = new List<Graph>();
            foreach (Graph graphPart in GraphParts)
            {
                int n = graphPart.Nodes.Count;
                int e = graphPart.Edges.Count;
                if (n < 5 || e < 9)
                {
                    partsToRemove.Add(graphPart);
                }
                else if (e > 3*n - 6)
                {
                    PlanarityResult = PlanarityResult.NonPlanar;
                }
            }

            foreach (Graph graph in partsToRemove)
            {
                GraphParts.Remove(graph);
            }

            if (GraphParts.Count == 0)
            {
                PlanarityResult = PlanarityResult.Planar;
            }

            yield return "PREPROCESS STEP 4: Removed planar components...";
        }

        // Main planarity testing function.
        // Should be called asynchronously to show status texts at each step.
        public IEnumerable<string> ApplyPlanarityTestStep()
        {
            int componentNo = 0; // This is just used to show text
            foreach (Graph graphPart in GraphParts)
            {
                componentNo++;
                // Find a circuit C of G
                // Actually we find cycles because with circuits (where nodes may be repeated),
                // there was a problem when detecting initial faces (there may be 3 faces)
                // and I could not find a case where finding cycle did not work.
                // It seems like the algorithm should say "cycle" instead of "circuit"
                CycleFinder cycleFinder = new CycleFinder();
                SubGraph = cycleFinder.FindCycle(graphPart);
                if (SubGraph == null)
                {
                    // Graph has no cycles, thus is planar.
                    continue;
                }
                yield return "TESTING PLANARITY: Found cycle for component " + componentNo;
                // Determine faces as two faces with all nodes as their boundary
                SubGraph.SetCycleFaces();
                bool embeddable = true;
                int f = 2;
                int e = graphPart.Edges.Count;
                int n = graphPart.Nodes.Count;

                while (f != e - n + 2 && embeddable)
                {
                    BridgeFinder bridgeFinder = new BridgeFinder();
                    // find each bridge B of G relative to Gi
                    List<Graph> bridges = bridgeFinder.FindBridges(graphPart, SubGraph);
                    // Find F(B,Gi) in the algorithm (which faces can the bridge be drawn on)
                    Dictionary<Graph, List<Face>> bridgeDrawableFaces = new Dictionary<Graph, List<Face>>();
                    foreach (Graph bridge in bridges)
                    {
                        bridgeDrawableFaces[bridge] = new List<Face>();
                        foreach (Face face in SubGraph.Faces)
                        {
                            if (bridge.PointsOfContract.All(p => face.Nodes.Contains(p)))
                            {
                                bridgeDrawableFaces[bridge].Add(face);
                            }
                        }
                        if (bridgeDrawableFaces[bridge].Count == 0)
                        {
                            // for some B, F(B,Gi)= ∅
                            embeddable = false;
                            PlanarityResult = PlanarityResult.NonPlanar;
                            break;
                        }
                    }
                    if (embeddable)
                    {
                        Graph selectedBridge = bridges.FirstOrDefault(b => bridgeDrawableFaces[b].Count == 1);
                        Face selectedFace;
                        if (selectedBridge != null)
                        {
                            // For some B, |F(B,Gi)| = 1 then f = F(B,Gi)
                            selectedFace = bridgeDrawableFaces[selectedBridge][0];
                        }
                        else
                        {
                            // Let B be any bridge and f be any face, f ∈F(B,Gi)
                            selectedBridge = bridges[0];
                            selectedFace = bridgeDrawableFaces[selectedBridge][0];
                        }
                        // Find a path Pi⊆B connecting two points of contact of B to Gi
                        PathFinder pathFinder = new PathFinder();
                        List<Edge> pathInBridge = pathFinder.FindPath(selectedBridge, selectedBridge.PointsOfContract[0], selectedBridge.PointsOfContract[1]);

                        // Draw Pi in the face f of Gi
                        SubGraph.AddBridgeAndSeperateFace(pathInBridge, selectedFace, new []{ selectedBridge.PointsOfContract[0], selectedBridge.PointsOfContract[1] });

                        yield return "TESTING PLANARITY: Added a bridge piece, total faces: " + f;
                        f++;
                    }
                }
                // If one of the components is Non-Planar, whole graph is non-planar
                if (PlanarityResult == PlanarityResult.NonPlanar) break;
            }
            if (PlanarityResult == PlanarityResult.Undetermined)
            {
                // All components are processed and none is Non-planar
                PlanarityResult = PlanarityResult.Planar;
            }
        }

    }
}