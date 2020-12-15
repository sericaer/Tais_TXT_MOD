using System.Linq;
using Parser.Semantic;

namespace GMData.Def
{
    public interface IDesc
    {
        string name { get; }
        string format { get; }
        string[] argv { get; }
    }

    public class Desc : IDesc
    {
        private GroupValue groupValue;

        public Desc(GroupValue groupValue)
        {
            this.groupValue = groupValue;
        }

        public string name => groupValue.First() as string;
        public string format => name;
        public string[] argv => groupValue.Skip(1).Select(x => x.ToString()).ToArray();
    }
}
