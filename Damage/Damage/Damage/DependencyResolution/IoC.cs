// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using StructureMap;
using Microsoft.Practices.ServiceLocation;
using Damage.Gadget;

namespace Damage.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");

            ObjectFactory.Initialize(x =>
                        {
                            //Scan current assembly
                            x.Scan(with =>
                                {
                                    with.TheCallingAssembly();
                                    with.WithDefaultConventions();
                                });
                            //                x.For<IExample>().Use<Example>();


                            //Scan gadgets folder
                            if (new System.IO.DirectoryInfo(System.IO.Path.Combine(binPath, "Gadgets")).Exists)
                            {
                                x.Scan(with =>
                                    {
                                        with.AssembliesFromPath(System.IO.Path.Combine(binPath, "Gadgets"));
                                        with.WithDefaultConventions();
                                        with.AddAllTypesOf<IGadget>();
                                    });
                            }
                            
                        });

            return ObjectFactory.Container;
        }
    }
}