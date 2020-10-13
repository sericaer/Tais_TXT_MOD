using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using Parser.Semantic;
using Parser.Syntax;

namespace GMData.Mod
{
    public class Option
    {
        public Desc desc
        {
            get
            {
                return new Desc(semantic.desc, Path.GetFileNameWithoutExtension(ownerName), index);
            }
        }

        public void Selected()
        {
            semantic.set?.Do();
        }

        public string Next
        {
            get
            {
                string rslt = semantic.next?.Rslt();
                if (rslt == null)
                {
                    var randomGroup = semantic.nextRandom?.Calc().Where(x=>x.value > 0);
                    if(randomGroup != null)
                    {
                        rslt = Tools.GRandom.CalcGroup(randomGroup);
                    }
                }

                return rslt == null ? "" : rslt;
            }
        }

        public (string op, string left, SingleValue right)[] sets
        {
            get
            {
                return semantic.set.info;
            }
        }

        internal string ownerName;
        internal int index;
        internal Parser.Semantic.Option semantic;

        public class Desc : Text
        {
            public Desc(GroupValue groupValue, string fileName, int index) : base(groupValue, $"{fileName}_OPTION_{index}_DESC")
            {

            }
        }
    }
}
