using System;
using System.Collections;
using Exortech.NetReflector;

namespace ThoughtWorks.CruiseControl.Core.Sourcecontrol
{
	[ReflectorType("multi")]
	public class MultiSourceControl : ITemporaryLabeller, ISourceControl
	{
		private ISourceControl[] _sourceControls;

		[ReflectorArray("sourceControls", Required=true)]
		public ISourceControl[] SourceControls 
		{
			get 
			{
				if (_sourceControls == null)
					_sourceControls = new ISourceControl[0];

				return _sourceControls;
			}

			set { _sourceControls = value; }
		}

		public Modification[] GetModifications(DateTime from, DateTime to)
		{
			ArrayList modifications = new ArrayList();
			foreach (ISourceControl sourceControl in SourceControls)
			{
				Modification[] mods = sourceControl.GetModifications(from, to);
				if (mods != null)
				{
					modifications.AddRange(mods);
				}
			}

			return (Modification[]) modifications.ToArray(typeof(Modification));
		}

		public void LabelSourceControl( string label, IIntegrationResult result )
		{
			foreach (ISourceControl sourceControl in SourceControls)
			{
				sourceControl.LabelSourceControl(label, result);
			}
		}

		public void Run(IIntegrationResult result)
		{
			result.Modifications = GetModifications(result.LastModificationDate, DateTime.Now);
		}

		public void CreateTemporaryLabel()
		{
			foreach (ISourceControl sourceControl in SourceControls)
			{
				if ( sourceControl is ITemporaryLabeller )
				{
					( (ITemporaryLabeller) sourceControl ).CreateTemporaryLabel();
				}
			}
		}

		public void DeleteTemporaryLabel()
		{
			foreach (ISourceControl sourceControl in SourceControls)
			{
				if ( sourceControl is ITemporaryLabeller )
				{
					( (ITemporaryLabeller) sourceControl ).DeleteTemporaryLabel();
				}
			}
		}

		public void GetSource(IIntegrationResult result) 
		{
			foreach (ISourceControl sourceControl in SourceControls)
			{
				sourceControl.GetSource(result);
			}
		}

		public void Initialize(IProject project)
		{
		}

		public void Purge(IProject project)
		{
		}
	}
}
