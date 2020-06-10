using System;

namespace GetChineseNum
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input a Num string:");
            var nums = Console.ReadLine();
            //第一個參數為空 或 輸入 數量 兩字，在小數點後的念法會不同
            var result = TransToChineseString("", nums);
            var result2 = TransToChineseString("數量", nums);
            Console.WriteLine($"Result:{result}");
            Console.WriteLine($"Result2:{result2}");
            Console.ReadKey();
        }

        public static string TransToChineseString(string type, string Num)
        {
            try
            {
                string m_1, m_2, m_3, m_4, m_5, m_6, m_7, m_8, m_9;
                m_1 = Num;
                string numNum = "0123456789.";
                string numChina = "零壹貳參肆伍陸柒捌玖點";
                string numChinaWeigh = "個拾佰仟萬拾佰仟億拾佰仟萬";

                #region 過濾開頭0 ex:00123-->123
                if (Num.StartsWith("0"))
                {
                    var startIndex = 0;
                    for (startIndex = 0; startIndex < Num.Length; startIndex++)
                    {
                        //char->'0' 數字為48
                        if (Num[startIndex] > 48) break;
                    }
                    Num = Num.Substring(startIndex, Num.Length - startIndex);
                }
                Console.WriteLine($"***Num-{Num}");
                #endregion

                #region 尾數(小數點後)處理 ex:345 --> 345.00
                if (!Num.Contains("."))
                    Num += ".00";
                else//123.234  123.23 123.2
                    Num = Num.Substring(0, Num.IndexOf(".") + 1 + (Num.Split('.')[1].Length > 2 ? 3 : Num.Split('.')[1].Length));
                Console.WriteLine($"***Num***-{Num}");
                #endregion

                m_1 = Num;
                m_2 = m_1;
                m_3 = m_4 = "";

                #region 將數字轉為大寫國字 ex: m_2:1234-> 壹貳叁肆
                for (int i = 0; i < 11; i++)
                {
                    m_2 = m_2.Replace(numNum.Substring(i, 1), numChina.Substring(i, 1));
                }
                Console.WriteLine($"***m_2-{m_2}");
                #endregion

                #region 將數字位數轉換大寫 ex:m_3:佰拾萬仟佰拾個
                int iLen = m_1.Length;
                if (m_1.IndexOf(".") > 0)
                    iLen = m_1.IndexOf(".");//獲取整數位數
                for (int j = iLen; j >= 1; j--)
                    m_3 += numChinaWeigh.Substring(j - 1, 1);
                Console.WriteLine($"***m_3-{m_3}");
                #endregion

                #region 將數字大寫與位數大寫合併 ex:m_4:m2+m3
                for (int i = 0; i < m_3.Length; i++)
                    m_4 += m_2.Substring(i, 1) + m_3.Substring(i, 1);
                Console.WriteLine($"***m_4-{m_4}");
                #endregion

                #region 合併後的大寫數字做贅字處理 ex:m_5:m4去"0"後拾佰仟
                m_5 = m_4;
                m_5 = m_5.Replace("零拾", "零");
                m_5 = m_5.Replace("零佰", "零");
                m_5 = m_5.Replace("零仟", "零");
                Console.WriteLine($"***m_5-{m_5}");
                
                //m_6:00-> 0,000-> 0
                m_6 = m_5;
                for (int i = 0; i < iLen; i++)
                    m_6 = m_6.Replace("零零", "零");
                Console.WriteLine($"***m_6-{m_6}");

                //m_7:6行去億,萬,個位"0"
                m_7 = m_6;
                m_7 = m_7.Replace("億零萬零", "億零");
                m_7 = m_7.Replace("億零萬", "億零");
                m_7 = m_7.Replace("零億", "億");
                m_7 = m_7.Replace("零萬", "萬");
                if (m_7.Length > 2)
                    m_7 = m_7.Replace("零個", "個");
                Console.WriteLine($"***m_7-{m_7}");
                #endregion

                #region 大寫數字做整數/小數點處理
                //m_8:7行+2行小數-> 數目
                m_8 = m_7;
                m_8 = m_8.Replace("個", "");
                if (m_2.Substring(m_2.Length - 3, 3) != "點零零")
                    m_8 += m_2.Substring(m_2.Length - 3, 3);
                Console.WriteLine($"***m_8-{m_8}");
               
                //m_9:7行+2行小數-> 價格
                m_9 = m_7;
                m_9 = m_9.Replace("個", "元");

                if (m_2.Substring(m_2.Length - 3, 3) != "點零零")
                {
                    m_9 += m_2.Substring(m_2.Length - 2, 2);
                    m_9 = m_9.Insert(m_9.Length - 1, "角");
                    m_9 += "分";
                }
                else m_9 += "整";

                if (m_9 != "零元整")
                    m_9 = m_9.Replace("零元", "");
                m_9 = m_9.Replace("零分", "整");
                #endregion

                return (type == "數量") ? m_8 : m_9;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
