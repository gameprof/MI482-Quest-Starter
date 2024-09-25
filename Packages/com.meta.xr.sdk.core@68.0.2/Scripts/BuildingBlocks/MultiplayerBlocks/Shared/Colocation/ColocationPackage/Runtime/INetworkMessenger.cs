/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace Meta.XR.MultiplayerBlocks.Colocation
{
    /// <summary>
    ///   An interface used to send RPC calls used for a player to join someone else's colocated space
    /// </summary>
    internal interface INetworkMessenger
    {
        event Action<ShareAndLocalizeParams> AnchorShareRequestReceived;
        event Action<ShareAndLocalizeParams> AnchorShareRequestCompleted;

        public void SendAnchorShareRequest(ulong targetPlayerId, ShareAndLocalizeParams shareAndLocalizeParams);
        public void SendAnchorShareCompleted(ulong targetPlayerId, ShareAndLocalizeParams shareAndLocalizeParams);
        public void RegisterLocalPlayer(ulong localPlayerId);
    }
}
