﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Resolve;
using JetBrains.ReSharper.Psi.Resolve;

namespace GammaJul.ReSharper.EnhancedTooltip.Presentation {

	internal static class CSharpColorizerExtensions {

		public static void AppendPlainText([NotNull] this CSharpColorizer colorizer, [CanBeNull] string text) {
			colorizer.AppendText(text, null);
		}

		public static void AppendKeyword([NotNull] this CSharpColorizer colorizer, [CanBeNull] string keyword) {
			colorizer.AppendText(keyword, VsHighlightingAttributeIds.Keyword);
		}

		public static void AppendElementKind([NotNull] this CSharpColorizer colorizer, [CanBeNull] IDeclaredElement element) {
			colorizer.AppendText(element.GetElementKindString(), null);
		}

		public static void AppendCandidates([NotNull] this CSharpColorizer colorizer, [CanBeNull] IReference reference) {
			if (reference == null)
				return;

			IResolveResult resolveResult = reference.Resolve().Result;
			var liftedResolveResult = resolveResult as LiftedResolveResult;

			// ReSharper disable once CoVariantArrayConversion
			IList<IDeclaredElement> candidates = liftedResolveResult != null ? liftedResolveResult.LiftedCandidates : resolveResult.Candidates;
			
			IList<ISubstitution> substitutions = resolveResult.CandidateSubstitutions;

			for (int i = 0; i < candidates.Count; ++i) {
				colorizer.AppendPlainText(Environment.NewLine);
				colorizer.AppendPlainText("  ");
				colorizer.AppendDeclaredElement(candidates[i].EliminateDelegateInvokeMethod(), substitutions[i], PresenterOptions.FullWithoutParameterNames);
			}
		}

		public static void AppendParameterKind([NotNull] this CSharpColorizer colorizer, ParameterKind parameterKind) {
			switch (parameterKind) {
				case ParameterKind.VALUE:
					colorizer.AppendPlainText("value");
					return;
				case ParameterKind.REFERENCE:
					colorizer.AppendKeyword("ref");
					return;
				case ParameterKind.OUTPUT:
					colorizer.AppendKeyword("out");
					return;
				default:
					colorizer.AppendPlainText("value");
					return;
			}
		}
		
		public static void AppendAccessorKind([NotNull] this CSharpColorizer colorizer, AccessorKind accessorKind) {
			switch (accessorKind) {
				case AccessorKind.ADDER:
					colorizer.AppendKeyword("add");
					return;
				case AccessorKind.REMOVER:
					colorizer.AppendKeyword("remove");
					return;
				case AccessorKind.GETTER:
					colorizer.AppendKeyword("get");
					return;
				case AccessorKind.SETTER:
					colorizer.AppendKeyword("set");
					return;
				case AccessorKind.RAISER:
					colorizer.AppendPlainText("raise");
					return;
				case AccessorKind.UNKNOWN:
					colorizer.AppendPlainText("unknown");
					return;
			}
		}

	}

}