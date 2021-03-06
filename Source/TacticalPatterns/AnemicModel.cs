using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Monads;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public abstract class AnemicModel<TBoundedContext> : 
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IComplexKey _complexKey;
        private readonly IDictionary<string, IValueObject> _invariants;

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="complexKey">Составной ключ.</param>
        /// <param name="valueObjects">Словарь объект значений.</param>
        public AnemicModel(
            IComplexKey complexKey,
            IDictionary<string, IValueObject> invariants)
        {
            _complexKey = complexKey;
            // validating Aggregate Root.
            if (!invariants.ContainsKey("Root"))
            {
                throw new ArgumentException("Root must be Root.");
            }

            _invariants = invariants;
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="root">Корень агрегата.</param>
        /// <param name="invariants">Словарь объект значений.</param>
        public AnemicModel(
            IAggregateRoot<TBoundedContext> root,
            IDictionary<string, IValueObject> invariants)
            : this((IComplexKey)root, invariants.Do(vo => vo.Add("Root", root)))
        {
        }

        /// <summary>
        /// Конструктор анемичной модели.
        /// Потомки должны реализовать свой конструктор с валидацией объект значений.
        /// </summary>
        /// <param name="complexKey">Составной ключ.</param>
        /// <param name="invariants">Словарь объект значений.</param>
        public AnemicModel(
            IComplexKey complexKey,
            params IValueObject[] invariants)
            : this(complexKey, invariants.ToImmutableDictionary(k => k.GetType().Name, v => v))
        {
        }

        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        public Guid Id => _complexKey.Id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _complexKey.Version;

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        public Guid CorrelationToken => _complexKey.CorrelationToken;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> Invariants => _invariants;
    }
}
