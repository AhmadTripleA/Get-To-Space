using System;
using System.Collections.Generic;

interface IPowerBase
{
    public int PowerGridId { get; set; }
}

interface IPowerProducer : IPowerBase
{
    public void ProducePower();
}

interface IPowerConsumer : IPowerBase
{
    public void ConsumePower();
}