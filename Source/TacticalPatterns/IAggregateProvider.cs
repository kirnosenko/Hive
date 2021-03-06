﻿using System;
using System.Collections.Generic;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IAggregateProvider<TBoundedContext> :
        IAggregateProvider<TBoundedContext, Aggregate<TBoundedContext>, IAggregate<TBoundedContext>>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAggregate">Корневой агрегат контекста.</typeparam>
    public interface IAggregateProvider<TBoundedContext, TAggregate> :
        IAggregateProvider<TBoundedContext, TAggregate, TAggregate>
        where TAggregate : class, IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAggregate">Корневой агрегат контекста.</typeparam>
    /// <typeparam name="TModel">Тип модели.</typeparam>
    public interface IAggregateProvider<TBoundedContext, TModel, TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
        where TModel : class, TAggregate
    {
        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="command">Команда иницировавшая операцию.</param>
        /// <returns>Коллекция агрегатов удовлетворяющая условию.</returns>
        TAggregate GetAggregateByIdAndVersion(Guid id, CommandToAggregate command);

        /// <summary>
        /// Получение агрегата(ов) по идентификатору и версии.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия аргрегата.</param>
        /// <param name="command">Команда иницировавшая операцию.</param>
        /// <returns>Коллекция агрегатов удовлетворяющая условию.</returns>
        TAggregate GetAggregateByIdAndVersion(Guid id, int version, CommandToAggregate command);
        
        /// <summary>
        /// Создание экземпляра агрегата по соответствующей dto.
        /// </summary>
        /// <typeparam name="TIn">Тип dto команды.</typeparam>
        /// <param name="anemicModel">Анемичная модель.</param>
        /// <param name="command">Команда иницировавшая операцию.</param>
        /// <returns>Новый экземпляр команды КА.</returns>
        TAggregate NewAggregate(IAnemicModel<TBoundedContext> anemicModel, CommandToAggregate command);

        /// <summary>
        /// Сохранение изменений.
        /// </summary>
        /// <param name="aggregate">Агрегат изменине в котором нужно сохранить.</param>
        /// <returns>Агрегат с сохраненными изменениями.</returns>
        void SaveChanges(TAggregate aggregate);
    }
}
