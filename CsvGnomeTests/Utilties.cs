﻿using CsvGnome;
using CsvGnome.Components;
using CsvGnome.Fields;
using CsvGnome.Components.Combinatorial;
using CsvGnomeScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public static class Utilties
    {
        private static void CreateAll(
            out TestReporter reporter,
            out Cache cache,
            out Factory combinatorialFactory,
            out Deleter combinatorialDeleter,
            out FieldBrain fieldBrain,
            out Manager manager,
            out Interpreter interpreter)
        {
            reporter = new TestReporter();
            cache = new Cache();
            combinatorialFactory = new Factory(cache);
            combinatorialDeleter = new Deleter(cache);
            fieldBrain = new FieldBrain(combinatorialFactory, combinatorialDeleter);
            manager = new Manager();
            interpreter = new Interpreter(fieldBrain, reporter, manager, new TestContext(), new CsvGnome.Date.Provider(), new TestConfigurationProvider());
        }

        public static void InterpreterTestInit(
            out FieldBrain fieldBrain,
            out Interpreter interpreter)
        {
            TestReporter reporter;
            Cache cache;
            Factory combinatorialFactory;
            Deleter combinatorialDeleter;
            Manager manager;

            CreateAll(
                out reporter,
                out cache,
                out combinatorialFactory,
                out combinatorialDeleter,
                out fieldBrain,
                out manager,
                out interpreter);
        }

        /// <summary>
        /// Create an interpreter and combinatorial cache
        /// </summary>
        public static void InterpreterTestInit(
            out Cache cache,
            out Interpreter interpreter)
        {
            TestReporter reporter;
            Factory combinatorialFactory;
            Deleter combinatorialDeleter;
            FieldBrain fieldBrain;
            Manager manager;

            CreateAll(
                out reporter,
                out cache,
                out combinatorialFactory,
                out combinatorialDeleter,
                out fieldBrain,
                out manager,
                out interpreter);
        }

        public static IncrementingComponent NewIncrementingComponent(IContext ctx, int start, int? increment = null, int? every = null)
        {
            if (every.HasValue) return new IncrementingComponent(ctx, start, increment.Value, every.Value);
            if (increment.HasValue) return new IncrementingComponent(ctx, start, increment.Value);
            return new IncrementingComponent(ctx, start);
        }

        public static IncrementingComponent NewIncrementingComponent(int start, int? increment = null, int? every = null)
        {
            return NewIncrementingComponent(new TestContext(), start, increment, every);
        }
    }
}
