// ------------------------------------------------------------------------
// Husky - Call of Duty BSP Extractor
// Copyright (C) 2018 Philip/Scobalula
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;

namespace Husky
{
    /// <summary>
    /// A class to hold IWMap Info and Logic
    /// </summary>
    public class IWMap
    {
        /// <summary>
        /// A class to hold a .MAP Entity
        /// </summary>
        public class Entity
        {
            /// <summary>
            /// Entity KVPs
            /// </summary>
            public Dictionary<string, string> KeyValuePairs = new Dictionary<string, string>();

            /// <summary>
            /// Initializes an entity with a Classname
            /// </summary>
            public Entity(string className)
            {
                KeyValuePairs["classname"] = className;
            }

            /// <summary>
            /// Creates a Misc/Prop Model
            /// </summary>
            /// <param name="modelName">Name of the Model Asset</param>
            /// <param name="origin">XYZ Origin</param>
            /// <param name="angles">XYZ Angles</param>
            /// <param name="modelScale">Model Scale</param>
            /// <returns>Resulting entity</returns>
            public static Entity CreateMiscModel(string modelName, Vector3 origin, Vector3 angles, float modelScale)
            {
                // Create new entity
                var result = new Entity("misc_model");
                // Add properties
                result.KeyValuePairs["model"] = modelName;
                result.KeyValuePairs["origin"] = String.Format("{0:0.0000} {1:0.0000} {2:0.0000}", origin.X, origin.Y, origin.Z);
                result.KeyValuePairs["angles"] = String.Format("{1:0.0000} {2:0.0000} {0:0.0000}", angles.X, angles.Y, angles.Z);
                result.KeyValuePairs["modelscale"] = modelScale.ToString();
                // Ship her back
                return result;
            }
        }

        /// <summary>
        /// Map entities
        /// </summary>
        public List<Entity> Entities = new List<Entity>();

        /// <summary>
        /// Initializes an IW Map with a Basic Worldspawn Entity
        /// </summary>
        public IWMap()
        {
            // Create Worldspawn Entity
            var worldspawn = new Entity("worldspawn");
            // Set properties
            worldspawn.KeyValuePairs["fsi"]                   = "default";
            worldspawn.KeyValuePairs["gravity"]               = "800";
            worldspawn.KeyValuePairs["lodbias"]               = "default";
            worldspawn.KeyValuePairs["lutmaterial"]           = "luts_t7_default";
            worldspawn.KeyValuePairs["numOmniShadowSlices"]   = "24";
            worldspawn.KeyValuePairs["numSpotShadowSlices"]   = "64";
            worldspawn.KeyValuePairs["sky_intensity_factor0"] = "1";
            worldspawn.KeyValuePairs["sky_intensity_factor1"] = "1";
            worldspawn.KeyValuePairs["state_alias_1"]         = "State 1";
            worldspawn.KeyValuePairs["state_alias_2"]         = "State 2";
            worldspawn.KeyValuePairs["state_alias_3"]         = "State 3";
            worldspawn.KeyValuePairs["state_alias_4"]         = "State 4";
            // Add it
            Entities.Add(worldspawn);
        }

        /// <summary>
        /// Dumps whatever we can to a .MAP file
        /// </summary>
        /// <param name="fileName">File Name</param>
        public void DumpToMap(string fileName)
        {
            // Create output stream
            using (var writer = new StreamWriter(fileName))
            {
                // Write basic header
                writer.WriteLine("iwmap 4");
                writer.WriteLine("\"script_startingnumber\" 0");
                writer.WriteLine("\"000_Global\" flags  active");
                writer.WriteLine("\"The Map\" flags expanded ");

                // Write entitys
                for (int i = 0; i < Entities.Count; i++)
                {
                    // Write Index Comment
                    writer.WriteLine("// entity {0}", i);
                    // Write initial bracket
                    writer.WriteLine("{");
                    // Write KVPs
                    foreach (var kvp in Entities[i].KeyValuePairs)
                        writer.WriteLine("\"{0}\" \"{1}\"", kvp.Key, kvp.Value);
                    // Write end bvracket
                    writer.WriteLine("}");
                }
            }
        }
    }
}
