using System.Collections.Generic;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.util {

    using Gaussian = org.neuroph.core.transfer.Gaussian;
    using Linear = org.neuroph.core.transfer.Linear;
    using Log = org.neuroph.core.transfer.Log;
    using Ramp = org.neuroph.core.transfer.Ramp;
    using Sgn = org.neuroph.core.transfer.Sgn;
    using Sigmoid = org.neuroph.core.transfer.Sigmoid;
    using Sin = org.neuroph.core.transfer.Sin;
    using Step = org.neuroph.core.transfer.Step;
    using Tanh = org.neuroph.core.transfer.Tanh;
    using Trapezoid = org.neuroph.core.transfer.Trapezoid;

    /// <summary>
    /// Contains transfer functions types and labels.
    /// </summary>
    public enum TransferFunctionType {
        LINEAR,
        RAMP,
        STEP,
        SIGMOID,
        TANH,
        GAUSSIAN,
        TRAPEZOID,
        SGN,
        SIN,
        LOG
    }
}