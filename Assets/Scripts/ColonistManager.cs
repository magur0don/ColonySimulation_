using UnityEngine;
using UnityEngine.InputSystem;

public class ColonistManager : MonoBehaviour
{
    /// <summary>
    /// []�͔z��ƌ����A��̕ϐ��̒��ŕ�����ColonistAI���Ǘ��ł��܂��B
    /// </summary>
    public ColonistAI[] Colonists;

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h�̃X�y�[�X�L�[�������ꂽ��
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // for����(�����l,�����l���w��̒l�ɂȂ�܂�,�����l�𑝌�������)�Ƃ���������
            // �����l���w��̒l�ɂȂ�܂ł̉񐔏������s��
            for (int i = 0; i < Colonists.Length; i++)
            {
                Colonists[i].State = ColonistAI.ColonistState.Mine;
            }
        }
    }
}
