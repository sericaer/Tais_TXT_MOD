using Godot;
using System;
using System.Collections.Generic;

namespace TaisGodot.Scripts
{
    public class TitleElement : Button
    {
        public enum Status
        {
            ascend,
            descend,
            no_sort
        }

        [Signal]
        public delegate void Sort(Status status);

        public string ascendImage;
        public string descendImage;

        public Status status;

        public TitleElement()
        {
            dictStatus = new Dictionary<Status, Status>()
            {
                { Status.no_sort, Status.descend },
                { Status.descend, Status.ascend },
                { Status.ascend, Status.descend },
            };

            status = Status.no_sort;
        }

        public void _on_Button_Pressed()
        {
            status = dictStatus[status];

            UpdateImage();

            EmitSignal(nameof(Sort), status);
        }

        private void UpdateImage()
        {
            switch (status)
            {
                case Status.descend:
                    break;
                case Status.ascend:
                    break;
                case Status.no_sort:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private Dictionary<Status, Status> dictStatus;
    }
}
