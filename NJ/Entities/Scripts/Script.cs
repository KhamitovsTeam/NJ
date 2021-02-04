using System;
using KTEngine;

namespace Chip
{
    /// <summary>
    /// Абстрактный класс для скриптов
    /// </summary>
    public abstract class Script : Entity
    {
        public abstract void OnTriggerEnter();
        public abstract void OnTriggerExit();
    }
}