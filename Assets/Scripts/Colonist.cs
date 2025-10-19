using UnityEngine;

public class Colonist : MonoBehaviour
{
    public string Name = "Taro";
    public float MoveSpeed = 2.0f;
    private Vector3 targetPosition = new Vector3(2, 0, 2);

    // �Q�[���̍ŏ��Ɉ�񂾂����s�����
    void Start()
    {
        Debug.Log($"{Name}���R���j�[�ɓ������܂���");
        SetRandomTarget();
    }

    // �Q�[���̎��s���A��Ɏ��s�����
    void Update()
    {
        // �ړ��������B�ړ�����̂ɕK�v�Ȃ͎̂����̈ʒu�ƁA�s���ׂ��ʒu�B
        // �����̈ʒu��targetPosition�܂ňړ�������
        // . �͐ڑ����ƍl���Ă��������OK
        transform.position = Vector3.MoveTowards(
            transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        // if���͂����A�����ʓ��̏�����������A�����ʂ̒��̏������s��
        // �����̈ʒu�ƁA�^�[�Q�b�g�̈ʒu��10cm���߂��Ȃ�����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ���̃^�[�Q�b�g�̈ʒu�����߂�
            SetRandomTarget();
        }
    }

    // ������ʒu�����߂鏈��������
    void SetRandomTarget()
    {
        targetPosition = new Vector3(
            Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
}