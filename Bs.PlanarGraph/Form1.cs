using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bs.PlanarGraph.Algorithm;
using Bs.PlanarGraph.Algorithm.Entities;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace Bs.PlanarGraph
{
    public partial class Form1 : Form
    {
        public Graph MainGraph { get; set; }
        public PlanarityTester PlanarityTester { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private string ConvertGraphToDot(Graph graph)
        {
            string dot = graph.Edges.Aggregate("Graph {", (current, edge) => current + (edge.Node1.Id + "--" + edge.Node2.Id + ";"));

            dot = dot.TrimEnd(';');
            dot += "}";

            return dot;
        }

        private string ConvertGraphToDot(List<Graph> graphs)
        {
            string dot = "Graph {";
            foreach (Graph graph in graphs)
            {
                dot += graph.Edges.Aggregate("", (current, edge) => current + (edge.Node1.Id + "--" + edge.Node2.Id + ";"));
            }

            dot = dot.TrimEnd(';');

            dot += "}";

            return dot;
        }

        private void VisualizeGraph(string dot)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);

            byte[] output = wrapper.GenerateGraph(dot, Enums.GraphReturnType.Png);

            graphView.Image = (Bitmap)((new ImageConverter()).ConvertFrom(output));
        }

        private void WriteStatus(string message)
        {
            textBoxStatus.Text = message;
        }

        private void ParseGraph(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Please fill input graph textbox.");
            }

            MainGraph = new Graph();

            input = input.Replace("Graph", "");
            input = input.Replace("{", "");
            input = input.Replace("}", "");
            input = input.Trim();
            string[] edges = input.Split(';');
            if (edges.Length < 2)
            {
                throw new Exception("Please enter a graph with at least 2 edges.");
            }
            foreach (string edge in edges)
            {
                string edgeText = edge.Trim();
                string[] nodes = edgeText.Split(new string[] {"--"}, StringSplitOptions.None);
                if (nodes.Length != 2)
                {
                    throw new Exception("Please enter a valid graph.");
                }
                if (nodes[0] == nodes[1]) continue; // No need to add loops to check planarity
                Node node1 = MainGraph.GetOrCreateNode(nodes[0]);
                Node node2 = MainGraph.GetOrCreateNode(nodes[1]);
                MainGraph.GetOrCreateEdge(node1, node2);
            }
        }

        private void buttonInitGraph_Click(object sender, EventArgs e)
        {
            try
            {
                ParseGraph(textBoxInitGraph.Text);
                string dot = ConvertGraphToDot(MainGraph);
                textBoxInitGraph.Text = dot;
                VisualizeGraph(dot);
                PlanarityTester = new PlanarityTester(MainGraph);
                WriteStatus("Graph is initialized! Click \"Apply Simplifying Step\" to apply preprocessing and simple tests.");
            }
            catch (Exception ex)
            {
                WriteStatus("Error visualizing graph: " + ex.Message);
            }
        }

        private void buttonSimplifyStep_Click(object sender, EventArgs e)
        {
            try
            {
                ApplySimplifyingStepsByShowingProcesses();
            }
            catch (Exception ex)
            {
                WriteStatus("Error simplifying graph: " + ex.Message);
            }
        }

        private async void ApplySimplifyingStepsByShowingProcesses()
        {
            buttonSimplifyStep.Enabled = false;
            buttonApplyPlanarityStep.Enabled = false;
            buttonInitGraph.Enabled = false;
            foreach (string preprocessMessage in PlanarityTester.ApplySimplifyingStep())
            {
                string dot = ConvertGraphToDot(PlanarityTester.GraphParts);
                VisualizeGraph(dot);
                WriteStatus(preprocessMessage);
                await Task.Delay(2000);
            }

            bool canSimplifyAgain = PlanarityTester.SimplifyResult == SimplifyResult.CanSimplifyAgain;
            if (PlanarityTester.PlanarityResult == PlanarityResult.NonPlanar)
            {
                WriteStatus("Graph is NON-PLANAR! This is determined in the simplifying step.");
            }
            else if (PlanarityTester.PlanarityResult == PlanarityResult.Planar)
            {
                WriteStatus("Graph is PLANAR! This is determined in the simplifying step.");
            }
            else
            {
                WriteStatus(canSimplifyAgain
                    ? "Graph is simplified! It may be further simplified, click \"Apply Simplifying Step\" again."
                    : "Graph is simplified! It cannot be simplified anymore. Click \"Apply Planarity Test Step\" button.");
            }
            buttonSimplifyStep.Enabled = true;
            buttonApplyPlanarityStep.Enabled = true;
            buttonInitGraph.Enabled = true;
        }
    }
}
