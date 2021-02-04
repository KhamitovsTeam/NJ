using KTEngine;

namespace Chip
{
    public abstract class MenuItem : Entity
    {
        private bool selected;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value)
                    return;
                selected = value;
                if (selected)
                    OnSelect();
                else
                    OnDeselect();
            }
        }

        protected abstract void OnSelect();

        protected abstract void OnDeselect();

        protected abstract void OnConfirm();
    }
}