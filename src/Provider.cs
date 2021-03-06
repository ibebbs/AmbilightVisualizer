﻿using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ambilight
{
    public interface IProvider
    {
        IObservable<Processed.IData> Processed { get; }
    }

    internal class PollingProvider : IProvider
    {
        public PollingProvider(IJointSpaceClient client)
        {
            Processed = Observable.Interval(TimeSpan.FromMilliseconds(50)).SelectMany(_ => client.GetAmbilightProcessed()).Publish().RefCount();
        }

        public IObservable<Processed.IData> Processed { get; private set; }
    }

    internal class FastProvider : IProvider
    {
        public FastProvider(IJointSpaceClient client)
        {
            Subject<Unit> observed = new Subject<Unit>();

            IObservable<Unit> processedTrigger = Observable.When(Observable.And(observed.StartWith(Unit.Default), Observable.Interval(TimeSpan.FromMilliseconds(10)).StartWith(TaskPoolScheduler.Default.Now.Ticks)).Then((x, y) => x));

            Processed = processedTrigger.SelectMany(_ => client.GetAmbilightProcessed()).Do(_ => observed.OnNext(Unit.Default)).Publish().RefCount();
        }

        public IObservable<Processed.IData> Processed { get; private set; }
    }
}
