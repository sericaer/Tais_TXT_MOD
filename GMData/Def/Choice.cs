using System;
using System.Linq;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    public interface IChoice
    {
        IDesc desc {get;}
        IRandomEvents randomEvent { get; }
        ISelect set { get; }
        IGEvent CalcRandomEvent(object obj);
    }

    public class Choice : IChoice
    {
        public IDesc desc => new Desc(_desc);
        public IRandomEvents randomEvent => _randomEvent;
        public ISelect set => _set;

        [SemanticProperty("desc")]
        internal GroupValue _desc;

        [SemanticProperty("random_event")]
        internal RandomEvents _randomEvent; 

        [SemanticProperty("select")]
        internal Select _set;

        public IGEvent CalcRandomEvent(object obj)
        {
            var randomGroup = randomEvent?.Calc().Where(x => x.value > 0);

            if (randomGroup != null)
            {
                var randomEvent = Tools.GRandom.CalcGroup(randomGroup);
                var eventObj = GMRoot.modder.FindEvent(randomEvent);
                eventObj.objTuple = new Tuple<string, object>("risk", obj);
                return eventObj;
            }

            return null;
        }
    }
}
