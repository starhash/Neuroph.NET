﻿/// <summary>
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

namespace org.neuroph.core.learning.error {

    /// <summary>
    /// Interface for calculating total network error during the learning.
    /// Custom error types  can be implemented.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    public interface ErrorFunction {

        /// <summary>
        /// Return total network error </summary>
        /// <returns> total network error </returns>
        double TotalError { get; }

        /// <summary>
        /// Sets total error to zero
        /// </summary>
        void reset();

        /// <summary>
        /// Calculates pattern error for given predicted and target output
        /// </summary>
        double[] calculatePatternError(double[] predictedOutput, double[] targetOutput);

    }

}