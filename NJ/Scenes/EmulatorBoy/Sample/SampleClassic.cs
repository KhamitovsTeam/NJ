namespace Chip
{
    /// <summary>
    /// Пример игры для аркадного кабинета в ракете
    /// </summary>
    public class SampleClassic : Classic
    {
        // ~ sample ~
        // Ural Khamitov

        // Переменные

        /// <summary>
        /// Инициализация объектов, необходимых для игры. Вызывается при нажатии Insert Coin в игровом автомате
        /// </summary>
        /// <param name="emulator">Сюда передаётся инстанс "эмулятора"</param>
        public override void Init(Emulator emulator)
        {
            base.Init(emulator);
            // Добавляем объекты в память "эмулятора"
            //Objects.Add();
        }

        /// <summary>
        /// Уничтожение всех объектов после выхода из игры. Вызывается при нажатии Esc в игровом автомате. Закрывает текущую игру.
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
        }

        /// <summary>
        /// Обновление игровой логики
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Отрисовка объектов
        /// </summary>
        public override void Draw()
        {
            base.Draw();
        }
    }
}