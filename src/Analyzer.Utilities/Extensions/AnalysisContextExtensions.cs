﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer.Utilities
{
    public static class AnalysisContextExtensions
    {
#if USE_INTERNAL_IOPERATION_APIS
        // By pass the IOperation feature flag check if building analyzers VSIX.
        private const string RegisterOperationActionMethodName = "RegisterOperationActionImmutableArrayInternal";
        private const string RegisterOperationBlockActionMethodName = "RegisterOperationBlockActionInternal";
        private const string RegisterOperationBlockStartActionMethodName = "RegisterOperationBlockStartActionInternal";
        private const string GetOperationMethodName = "GetOperationInternal";

        // AnalysisContext.RegisterOperationAction
        private static MethodInfo s_registerOperationActionOnAnalysisContext =
            typeof(AnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationActionMethodName);

        // AnalysisContext.RegisterOperationBlockAction
        private static MethodInfo s_registerOperationBlockActionOnAnalysisContext =
            typeof(AnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationBlockActionMethodName);

        // AnalysisContext.RegisterOperationBlockStartAction
        private static MethodInfo s_registerOperationBlockStartActionOnAnalysisContext =
            typeof(AnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationBlockStartActionMethodName);

        // CompilationStartAnalysisContext.RegisterOperationAction
        private static MethodInfo s_registerOperationActionOnCompilationStartAnalysisContext =
            typeof(CompilationStartAnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationActionMethodName);

        // CompilationStartAnalysisContext.RegisterOperationBlockAction
        private static MethodInfo s_registerOperationBlockActionOnCompilationStartAnalysisContext =
            typeof(CompilationStartAnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationBlockActionMethodName);

        // CompilationStartAnalysisContext.RegisterOperationBlockStartAction
        private static MethodInfo s_registerOperationBlockStartActionOnCompilationStartAnalysisContext =
            typeof(CompilationStartAnalysisContext).GetTypeInfo().GetDeclaredMethod(RegisterOperationBlockStartActionMethodName);

        // SemanticModel.GetOperation
        private static MethodInfo s_getOperationOnSemanticModel =
            typeof(SemanticModel).GetTypeInfo().GetDeclaredMethod(GetOperationMethodName);
#endif

        public static void RegisterOperationActionInternal(this AnalysisContext context, Action<OperationAnalysisContext> analyzerOperationCallback, params OperationKind[] operationKinds)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationActionOnAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback, ImmutableArray.Create(operationKinds) });
#else
            context.RegisterOperationAction(analyzerOperationCallback, operationKinds);
#endif
        }

        public static void RegisterOperationBlockActionInternal(this AnalysisContext context, Action<OperationBlockAnalysisContext> analyzerOperationCallback)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationBlockActionOnAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback });
#else
            context.RegisterOperationBlockAction(analyzerOperationCallback);
#endif
        }

        public static void RegisterOperationBlockStartActionInternal(this AnalysisContext context, Action<OperationBlockStartAnalysisContext> analyzerOperationCallback)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationBlockStartActionOnAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback });
#else
            context.RegisterOperationBlockStartAction(analyzerOperationCallback);
#endif
        }

        public static void RegisterOperationActionInternal(this CompilationStartAnalysisContext context, Action<OperationAnalysisContext> analyzerOperationCallback, params OperationKind[] operationKinds)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationActionOnCompilationStartAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback, ImmutableArray.Create(operationKinds) });
#else
            context.RegisterOperationAction(analyzerOperationCallback, operationKinds);
#endif
        }

        public static void RegisterOperationBlockActionInternal(this CompilationStartAnalysisContext context, Action<OperationBlockAnalysisContext> analyzerOperationCallback)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationBlockActionOnCompilationStartAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback });
#else
            context.RegisterOperationBlockAction(analyzerOperationCallback);
#endif
        }

        public static void RegisterOperationBlockStartActionInternal(this CompilationStartAnalysisContext context, Action<OperationBlockStartAnalysisContext> analyzerOperationCallback)
        {
#if USE_INTERNAL_IOPERATION_APIS
            s_registerOperationBlockStartActionOnCompilationStartAnalysisContext.Invoke(context, new object[] { analyzerOperationCallback });
#else
            context.RegisterOperationBlockStartAction(analyzerOperationCallback);
#endif
        }

        public static void RegisterOperationActionInternal(this OperationBlockStartAnalysisContext context, Action<OperationAnalysisContext> analyzerOperationCallback, params OperationKind[] operationKinds)
        {
            // No feature flag check on OperationBlockStartAnalysisContext.RegisterOperationAction, so we call it directly.
            context.RegisterOperationAction(analyzerOperationCallback, operationKinds);
        }

        public static IOperation GetOperationInternal(this SemanticModel model, SyntaxNode node, CancellationToken cancellationToken)
        {
#if USE_INTERNAL_IOPERATION_APIS
            return (IOperation)s_getOperationOnSemanticModel.Invoke(model, new object[] { node, cancellationToken });
#else
            return model.GetOperation(node, cancellationToken);
#endif
        }


        }
    }
