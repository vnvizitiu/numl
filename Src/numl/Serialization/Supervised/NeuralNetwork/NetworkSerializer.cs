﻿using System;
using System.IO;
using System.Linq;
using numl.Serialization;
using System.Collections.Generic;
using numl.Supervised.NeuralNetwork;

namespace numl.Serialization.Supervised.NeuralNetwork
{
    /// <summary>
    /// Neural Network serializer.
    /// </summary>
    public class NetworkSerializer : JsonSerializer<Network>
    {
        /// <summary>
        /// Deserializes the object from the stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        /// <returns>Network object.</returns>
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var network = (Network)this.Create();

                var nodes = reader.ReadArrayProperty().Value.OfType<Neuron>();
                var edges = reader.ReadArrayProperty().Value.OfType<Edge>();

                var linked = Network.LinkNodes(nodes, edges);

                network.In = linked.In;
                network.Out = linked.Out;

                return network;
            }
        }

        /// <summary>
        /// Writes the Neural Network object to the stream.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        /// <param name="value">Neural Network object to write.</param>
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var network = (Network)value;

                // write out nodes
                writer.WriteArrayProperty("Nodes", 
                        network.GetVertices().ToArray());

                // write out all edges
                writer.WriteArrayProperty("Edges", 
                        network.GetEdges().ToArray());
            }
        }
    }
}
