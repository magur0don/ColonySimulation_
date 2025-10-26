using UnityEngine;

public class ColonistAI : MonoBehaviour
{
    /// <summary>
    /// enum�^�Ő錾�����R���j�X�g�̏��
    /// </summary>
    public enum ColonistState
    {
        Idle,        // �ҋ@       
        Move,      // �ړ�
        Mine,       // �@��
        Sleep       // �A�Q
    }

    public ColonistState State;

    /// <summary>
    /// �R���j�X�g�̏�Ԃ�ύX���邽�߂̃^�C�}�[
    /// [SerializeField]�̂悤�Ȃ��̂𑮐�(Attribute)���Č����܂�
    /// </summary>
    [SerializeField]
    private float timer = 2f;

    public float MoveSpeed = 2.0f;
    private Vector3 targetPosition = new Vector3(2, 0, 2);

    void Start()
    {
        // �R���j�X�g�̏�Ԃ�Idle(�ҋ@)����n�߂�
        State = ColonistState.Idle;
        // �Ⴆ�Αҋ@�������ꍇ�A1�����Ĉړ��ɂł���
    }

    // Update is called once per frame
    void Update()
    {
        // 1�t���[���ɂ����������Ԃ�timer���猸�Z���Ă����܂�
        timer -= Time.deltaTime;

        // �����ʂ̒��̒l(�ϐ�)���g���ď����𕪊�(switch)�����܂�
        switch (State)
        {
            case ColonistState.Idle:
                // case��break�̊ԂɁAcase�̏ꍇ�̏���������
                // �����^�C�}�[��0�b�����������
                if (timer <= 0f)
                {
                    // �R���j�X�g�N�̏�Ԃ𓮂��Ƃ�����ԂɕύX
                    State = ColonistState.Move;
                    // �^�[�Q�b�g�|�W�V���������߂Ă�����
                    targetPosition = new Vector3(
            Random.Range(-5f, 5f), 0, Random.Range(-1f, 1f));
                    timer = 2f;
                }
                break;
            case ColonistState.Move:
                transform.position = Vector3.MoveTowards(
                    transform.position, targetPosition, MoveSpeed * Time.deltaTime);

                // if���͂����A�����ʓ��̏�����������A�����ʂ̒��̏������s��
                // �����̈ʒu�ƁA�^�[�Q�b�g�̈ʒu��10cm���߂��Ȃ�����
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    // ���̍s�����s��
                    State = ColonistState.Mine;
                    // �@�펞�Ԃ�1�b����5�b�܂ł̃����_��
                    timer = Random.Range(1f, 5f);
                }
                break;
            case ColonistState.Mine:
                // ���ō̌@�A�j���[�V�����Đ��̑���Ƀ��O���o�͂��܂�
                Debug.Log("Colonist is mining!");

                // ���t���[����]����������
                transform.Rotate(Vector3.up*30f*Time.deltaTime);

                if (timer <= 0f)
                {
                    // State = ColonistState.Idle;
                    // timer = 2f;
                    // timer��1�b�`15�b�Őݒ肵�܂��B
                    timer = Random.Range(1f, 15f);
                    // ���������������̏�����������
                    if (timer <= 3f)
                    {
                        // State��ColonistState.Sleep�ɕύX���Ă��������B
                        State = ColonistState.Idle;
                    }
                    else
                    {
                        State = ColonistState.Sleep;
                    }
                }
                break;
            case ColonistState.Sleep:
                // �����Atimer��0�b�����������AState��Idle�ɕύX���܂��傤�B
                if (timer <= 0f)
                {
                    State = ColonistState.Idle;
                    // timer��1�b����5�b�Őݒ肵�Ă��������B
                    timer = Random.Range(1f, 5f);
                }
                break;
        }
    }
}
