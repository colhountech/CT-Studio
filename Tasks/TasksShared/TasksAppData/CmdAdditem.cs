namespace TasksAppData
{
    public class CmdAdditem<T> : ICmd
    {
        public T Item { get; set; }
        public string[] Commands { get; set; } = new string[2];

        public CmdAdditem(T item)
        {

            this.Item = item;
            this.Commands[0] = "Add";
            this.Commands[1] = item!.GetType().ToString();
        }
    }
}