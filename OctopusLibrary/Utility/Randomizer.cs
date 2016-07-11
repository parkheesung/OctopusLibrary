using System;
using System.Text;

namespace OctopusLibrary.Utility
{
    public class Randomizer
    {
        public static string CreateGenerate(int size)
        {
            StringBuilder builder = new StringBuilder();

            Random Rnd = new Random();
            int tmp = 0;

            for (int i = 0; i < size; i++)
            {
                tmp = Rnd.Next(0, 62);

                // 랜덤 생성된 번호와, 해당 값의 사용여부가 일치 하는지 비교 판단 조건
                if (tmp < 10 || tmp >= 10 && tmp < 36 || tmp >= 36 && tmp < 62)
                {
                    // 어느 하나라도 조건에 성립되면 해당 문자를 출력
                    builder.Append(IntToChar(tmp));
                }
                else
                {
                    // 어떤 조건도 만족하지 못했을 경우, 루프 카운트를 -1 해서 다시 수행
                    i--;
                }
            }

            return builder.ToString();
        }

        public static string IntToChar(int num)
        {
            // 0-9 : 숫자
            // 10~35 : 소문자
            // 36~61 : 대문자

            // 0~9 : 숫자의 경우는 랜덤 생성번호 0~9를 그대로 사용
            if (num < 10)
            {
                return Convert.ToString(num);
            }
            // 10~35 : 랜덤 생성번호 10~35의 경우는 해당 숫자에 55를 더해서 그 아스키 값에 해당하는 문자를 출력
            else if (num >= 10 && num < 36)
            {
                num += 55;
                return string.Format("{0}", (char)num);
            }
            // 36~61 : 랜덤 생성번호 36~61의 경우는 해당 숫자에 61을 더해서 그 아스키 값에 해당하는 문자를 출력
            else if (num >= 36 && num < 62)
            {
                num += 61;
                return string.Format("{0}", (char)num);
            }
            // 해당되지 않는 랜덤번호 발생시 '#'으로 랜덤범위에 문제가 있음을 알림(랜덤범위 오류 검출용)
            else
            {
                return "#";
            }
        }
    }
}
