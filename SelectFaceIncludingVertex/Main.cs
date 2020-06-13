using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PEPlugin;
using PEPlugin.Pmx;

namespace SelectFaceIncludingVertex
{
    public class SelectFaceIncludingVertex : PEPluginClass
    {
        public SelectFaceIncludingVertex() : base()
        {
        }

        public override string Name
        {
            get
            {
                return "選択頂点を含む面を選択";
            }
        }

        public override string Version
        {
            get
            {
                return "0.0";
            }
        }

        public override string Description
        {
            get
            {
                return "選択頂点を含む面を選択";
            }
        }

        public override IPEPluginOption Option
        {
            get
            {
                // boot時実行, プラグインメニューへの登録, メニュー登録名
                return new PEPluginOption(false, true, "選択頂点を含む面を選択");
            }
        }

        public override void Run(IPERunArgs args)
        {
            try
            {
                var pmx = args.Host.Connector.Pmx.GetCurrentState();

                var selectedVertex = args.Host.Connector.View.PmxView.GetSelectedVertexIndices().Select(i => pmx.Vertex[i]).ToList();
                var allFace = pmx.Material.SelectMany(m => m.Faces);
                var includingFace = new List<IPXFace>();
                foreach (var v in selectedVertex)
                {
                    includingFace.AddRange(allFace.Where(f => f.GetVertexArray().Contains(v)));
                }
                var selectVertex = includingFace.SelectMany(f => f.GetVertexArray());
                selectVertex.Distinct();

                args.Host.Connector.View.PmxView.SetSelectedVertexIndices(selectVertex.Select(v=>pmx.Vertex.IndexOf(v)).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
