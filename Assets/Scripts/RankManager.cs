using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  이 클래스는 유저의 플레이 정보를 기록하는 클래스!
public class RankManager : MonoBehaviour
{
    [SerializeField] private Text[] m_RankTextes;

    //  유저가 게임을 승리했을 때
    //  1. 유저가 게임 실행 후 최초로 승리했을 때
    //  -> text 파일 생성 및 유저의 기록 저장, 로드
    //  2. 저장 파일 기록에 5줄 이상 있을 때 (우리가 만든 기록은 5줄 까지 지원하므로)

    public string GetRankTxtFile
    {
        get
        {
            //  폴더 위치를 확인한다!
            //  persistentDataPath ?
            //  위치는 %AppData%..\LocalLow\Matching-Card-Game...
            string dir = $"{Application.persistentDataPath}/Ranks";

            //  만약 폴더가 없다면 폴더 생성!
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            //  텍스트 파일 주소를 반환한다
            return $"{dir}/data.txt";
        }
    }

    public List<string> ReadRankFile()
    {
        //  텍스트 파일 주소를 받는다
        string txtPath = GetRankTxtFile;

        //  만약 텍스트 파일이 존재하지 않는다면 파일 생성
        if (!System.IO.File.Exists(txtPath))
        {
            System.IO.File.WriteAllText(txtPath, "");
            return null;
        }

        //  있다면 텍스트 파일을 읽는다
        //  StreamReader ?
        //  파일을 읽는데 도움되는 클래스
        //  이 클래스는 일화용으로도 쓸 수 있도록 지원하기 때문에
        //  일회용으로 쓸 거면 앞에 using 을 붙여 사용한다
        //  단, 모든 클래스를 일회용으로 쓸 수 있는게 아니다! 특정 몇몇만 사용할 수 있다
        //  ex) 클래스 이름 클릭 후 F12 누르면, 상속에 IDisposable 이 붙으면 가능
        List<string> list = new List<string>();
        //  List ?
        //  뭔가 동적으로 담을 수 있는 배열
        using (System.IO.StreamReader reader = new System.IO.StreamReader(txtPath))
        {
            while (!reader.EndOfStream)
            {
                list.Add(reader.ReadLine());
            }
        }

        return list;
    }

    public void WriteRankFile(float[] values)
    {
        string path = GetRankTxtFile;

        //  파일을 열어서 입력한다
        //  기존에 있는 데이터를 모두 삭제한 후 다시 작성한다
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
        {
            for (int i = 0; i < values.Length; ++i)
            {
                //  0 은 아직 기록이 없다는 뜻 이므로 ..
                if (values[i] != 0)
                    writer.WriteLine(values[i]);
            }
        }
    }

    public void OnShowRankLine(float newRank)
    {
        List<string> list = ReadRankFile();
        if (list == null)
        {
            //  랭크 없이 최초로 기록할 때
            m_RankTextes[0].text = newRank.ToString();
        }
        else
        {
            //  기존 기록이 있을 때
            //  float 형 배열 공간을 list 의 크기 + 1만큼 만들어 준다
            //  float 배열의 첫 번째 공간에 새로 추가된 랭크를 넣는다
            float[] ranks = new float[list.Count + 1];
            ranks[0] = newRank;
            for (int i = 1; i < ranks.Length; ++i)
            {
                //  string 을 float 형으로 변환한 뒤 float 배열에 넣어준다
                ranks[i] = float.Parse(list[i - 1]);
            }

            //  float 값들을 정렬한다
            System.Array.Sort(ranks);

            //  정렬한 값들을 랭크 보드의 텍스트들에게 넣어준다
            for (int i = 0; i < m_RankTextes.Length; ++i)
            {
                if (ranks.Length == i) break;
                m_RankTextes[i].text = ranks[i].ToString();
            }
        }

        //  텍스트를 저장할 수 있도록 float 배열 생성
        float[] values = new float[m_RankTextes.Length];
        for (int i = 0; i < m_RankTextes.Length; ++i)
        {
            var rank = m_RankTextes[i];

            //  UI.Text 안에 있는 text 를 검사하여
            //  안에 값이 있다면 저장할 수 있도록 float 배열 안에 값을 넣어주고
            //  만약 비어있다면 for 문을 멈춘다 (더 이상 값이 없기 때문)
            if (string.IsNullOrWhiteSpace(rank.text)) break;
            else
            {
                values[i] = float.Parse(rank.text);
            }
        }

        WriteRankFile(values);
        gameObject.SetActive(true);
    }

}