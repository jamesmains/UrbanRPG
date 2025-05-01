using System;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

// Todo: fix so that it works with the Dialogue Manager
namespace DefaultNamespace {
    [Serializable]
    public class ConversationStateCondition : Parent_House_Framework.Condition {

        public override bool IsConditionMet() {
            return ConversationManager.Instance.IsConversationActive;
        }
    }
}