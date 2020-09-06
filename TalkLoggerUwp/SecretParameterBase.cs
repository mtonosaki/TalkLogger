using System;
using System.Collections.Generic;
using System.Text;

namespace TalkLoggerUwp
{
    /// <summary>
    /// For your PoC, make a new class override below.
    /// </summary>
    public abstract class SecretParameterBase
    {
        public virtual string SubscriptionKey => "YourSubscriptionKey";
        public virtual string ServiceRegion => "YourServiceRegion";
    }
}
