﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs.Events;
using Cqrs.Authentication;
using Cqrs.Domain;

namespace Cqrs.Tests.Substitutes
{
	public class TestEventStore
		: IEventStore<ISingleSignOnToken>
	{
		public TestEventStore()
		{
			EmptyGuid = Guid.NewGuid();
			SavedEvents = new List<IEvent<ISingleSignOnToken>>();
		}

		public Guid EmptyGuid { get; private set; }

		public virtual void Save(Type aggregateRootType, IEvent<ISingleSignOnToken> @event)
		{
			SavedEvents.Add(@event);
		}

		public virtual IEnumerable<IEvent<ISingleSignOnToken>> Get<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1)
		{
			return Get(typeof(T),aggregateId, useLastEventOnly, fromVersion);
		}

		public virtual IEnumerable<IEvent<ISingleSignOnToken>> Get(Type aggregateType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1)
		{
			if (aggregateId == EmptyGuid || aggregateId == Guid.Empty)
			{
				return new List<IEvent<ISingleSignOnToken>>();
			}

			if (aggregateType == typeof (TestSaga))
			{
				return new List<ISagaEvent<ISingleSignOnToken>>
				{
					new SagaEvent<ISingleSignOnToken>
					(
						new TestAggregateDidSomethingElse {Id = Guid.NewGuid(), Version = 13}
					){Id = Guid.NewGuid(), Rsn = aggregateId, Version = 2},
					new SagaEvent<ISingleSignOnToken>
					(
						new TestAggregateDidSomething {Id = Guid.NewGuid(), Version = 26}
					){Id = Guid.NewGuid(), Rsn = aggregateId, Version = 3},
					new SagaEvent<ISingleSignOnToken>
					(
						new TestAggregateDidSomething {Id = Guid.NewGuid(), Version = 47}
					){Id = Guid.NewGuid(), Rsn = aggregateId, Version = 4}
				}
				.Where(x => x.Version > fromVersion);
			}
			return new List<IEvent<ISingleSignOnToken>>
			{
				new TestAggregateDidSomething {Id = aggregateId, Version = 2},
				new TestAggregateDidSomethingElse {Id = aggregateId, Version = 3},
				new TestAggregateDidSomething {Id = aggregateId, Version = 4}
			}
			.Where(x => x.Version > fromVersion);
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <see cref="IAggregateRoot{TAuthenticationToken}"/> of type <paramref name="aggregateRootType"/> with the ID matching the provided <paramref name="aggregateId"/> up to and including the provided <paramref name="version"/>.
		/// </summary>
		/// <param name="aggregateRootType"> <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</param>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="version">Load events up-to and including from this version</param>
		public IEnumerable<IEvent<ISingleSignOnToken>> GetToVersion(Type aggregateRootType, Guid aggregateId, int version)
		{
			return Get(aggregateRootType, aggregateId);
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <typeparamref name="T">aggregate root</typeparamref> with the ID matching the provided <paramref name="aggregateId"/> up to and including the provided <paramref name="version"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</typeparam>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="version">Load events up-to and including from this version</param>
		public IEnumerable<IEvent<ISingleSignOnToken>> GetToVersion<T>(Guid aggregateId, int version)
		{
			return GetToVersion(typeof(T), aggregateId, version);
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <see cref="IAggregateRoot{TAuthenticationToken}"/> of type <paramref name="aggregateRootType"/> with the ID matching the provided <paramref name="aggregateId"/> up to and including the provided <paramref name="versionedDate"/>.
		/// </summary>
		/// <param name="aggregateRootType"> <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</param>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="versionedDate">Load events up-to and including from this <see cref="DateTime"/></param>
		public IEnumerable<IEvent<ISingleSignOnToken>> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate)
		{
			return Get(aggregateRootType, aggregateId);
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <typeparamref name="T">aggregate root</typeparamref> with the ID matching the provided <paramref name="aggregateId"/> up to and including the provided <paramref name="versionedDate"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</typeparam>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="versionedDate">Load events up-to and including from this <see cref="DateTime"/></param>
		public IEnumerable<IEvent<ISingleSignOnToken>> GetToDate<T>(Guid aggregateId, DateTime versionedDate)
		{
			return GetToDate(typeof(T), aggregateId, versionedDate);
		}

		public IEnumerable<EventData> Get(Guid correlationId)
		{
			return Enumerable.Empty<EventData>();
		}

		public virtual void Save<T>(IEvent<ISingleSignOnToken> @event)
		{
			Save(typeof(T), @event);
		}

		private List<IEvent<ISingleSignOnToken>> SavedEvents { get; set; }
	}
}