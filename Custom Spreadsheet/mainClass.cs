using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace customProject
{
    /// <summary>
    /// Structure for parsed data
    /// </summary>
    public struct operation
    {
        public int oper;
        public int from_col;
        public int to_col;
        public int from_row;
        public int to_row;
    }

    /// <summary>
    /// Helping class providing number of helping function to main class
    /// </summary>
    public class helpingClass
    {

        /// <summary>
        /// Return int code for operations
        /// </summary>
        /// <param name="operation">Parsed Structure</param>
        /// <returns> Operation Code </returns>
        protected int ConvertOperationCode(string operation)
        {

            switch (operation.ToLower())
            {
                case "mean":
                    return 1;
                case "mode":
                    return 2;
                case "median":
                    return 3;
                case "sum":
                    return 4;

            }

            //if operation is  not proper throw the error
            throw new Exception("Invalid Format!");
        }

        /// <summary>
        /// Convert Basic operator to unique int code
        /// </summary>
        /// <param name="oper">Parsed Structure</param>
        /// <returns>Unique int code</returns>
        protected int ConvertOperatorCode(char oper)
        {
            switch (oper)
            {
                case '*':
                    return 5;
                case '/':
                    return 6;
                case '+':
                    return 7;
                case '-':
                    return 8;
            }

            //if operator is  not proper throw the error
            throw new Exception("Invalid Format!");
        }

        /// <summary>
        /// Convert column alphabet to column number
        /// </summary>
        /// <param name="ch">Column Alphabet</param>
        /// <returns>column integer</returns>
        protected int convertAlphabetToInt(char ch)
        {
            int num = char.ToUpper(ch) - 64;

            if(num <= 0)
            {
                throw new Exception("Invalid Format!");
            }

            return num;
        }

        /// <summary>
        /// Convert Int to column alphabet
        /// </summary>
        /// <param name="num"></param>
        /// <returns>column alphabet</returns>
        protected char convertInttoAlphabet(int num)
        {
            char myChar = (char)(num + 65);

            return myChar;
        }

        /// <summary>
        /// Convert string to alphabet, it convert row string(number) to int
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Row Integer</returns>
        protected int convertToInteger(string str)
        {
            
            if (int.TryParse(str, out int n))
            {
                
                if(n <= 26 && n>=0)
                {
                    return n;
                }
                else
                {
                    //if number is not in range of 0-26 throw error
                    throw new Exception("Invalid Format!");
                }
            }
            else 
                
            {
                throw new Exception("Invalid Format!");
            }
        }

        /// <summary>
        /// It split operator into row ,column and operator
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ch"></param>
        /// <param name="from_col"></param>
        /// <param name="from_row"></param>
        /// <param name="to_col"></param>
        /// <param name="to_row"></param>
        protected void splitOperator(string str, char ch, out int from_col, out int from_row, out int to_col, out int to_row)
        {
            var row_column = str.Split(ch);

            if(row_column.Length != 2)
            {
                throw new Exception("Invalid Format!");
            }

            from_col = convertAlphabetToInt(row_column[0][0]);
            from_row = convertToInteger(row_column[0].Substring(1));

            to_col = convertAlphabetToInt(row_column[1][0]);
            to_row = convertToInteger(row_column[1].Substring(1));
        }


        /// <summary>
        /// it calculate the min and max range of the operated area.
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="from_row"></param>
        /// <param name="from_col"></param>
        /// <param name="to_row"></param>
        /// <param name="to_col"></param>
        protected void getRowColumn(operation oper, out int from_row, out int from_col, out int to_row, out int to_col)
        {
            

            if (oper.to_row > oper.from_row)
            {
                to_row = oper.to_row;
                from_row = oper.from_row;
            }
            else
            {
                to_row = oper.from_row;
                from_row = oper.to_row;
            }

            if(oper.to_col > oper.from_col)
            {
                to_col = oper.to_col;
                from_col = oper.from_col;
            }
            else
            {
                to_col = oper.from_col;
                from_col = oper.to_col;
            }
        }

        /// <summary>
        /// Iterate the whole grid to find the cells between defined region
        /// </summary>
        /// <param name="content"></param>
        /// <param name="from_col"></param>
        /// <param name="from_row"></param>
        /// <param name="to_col"></param>
        /// <param name="to_row"></param>
        /// <returns></returns>
        protected List<double> iterateGrid(Grid content, int from_col, int from_row, int to_col, int to_row)
        {

            List<double> list = new List<double>();
            foreach (UIElement item in content.Children)
            {
                if (Grid.GetRow(item) >= (from_row-1) && Grid.GetColumn(item) >= (from_col-1) && Grid.GetRow(item) < to_row && Grid.GetColumn(item) < to_col)
                {
                    var text = (TextBox)item;
                    if (double.TryParse(text.Text, out double n))
                    {
                        list.Add(n);
                    }
                    else if (text.Text != "")
                    {
                        MessageBox.Show("Operation cant complete becuase of non numeric values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new Exception("None Numeric Value!");
                    }
                }
            }
        
            return list;
        }
        /// <summary>
        /// Iterate the whole grid to find the cells between defined region for single two cells i.e A1*A2
        /// </summary>
        /// <param name="content"></param>
        /// <param name="from_col"></param>
        /// <param name="from_row"></param>
        /// <param name="to_col"></param>
        /// <param name="to_row"></param>
        /// <returns></returns>
        protected List<double> iterateGrid(Grid content, int from_col, int from_row, int to_col, int to_row,out List<string> labels)
        {

            List<double> list = new List<double>();
            List<string> label = new List<string>();
            foreach (UIElement item in content.Children)
            {
                if (Grid.GetRow(item) >= (from_row - 1) && Grid.GetColumn(item) >= (from_col - 1) && Grid.GetRow(item) < to_row && Grid.GetColumn(item) < to_col)
                {
                    var text = (TextBox)item;
                    if (double.TryParse(text.Text, out double n))
                    {
                        list.Add(n);
                        label.Add(convertInttoAlphabet(Grid.GetColumn(item)) + (Grid.GetRow(item)+1).ToString());

                    }
                    else if (text.Text != "")
                    {
                        MessageBox.Show("Operation cant complete becuase of non numeric values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new Exception("None Numeric Value!");
                    }
                }
            }
            labels = label;
            return list;
        }
        /// <summary>
        /// Iterate the whole grid to find the cells for graph
        /// </summary>
        /// <param name="content"></param>
        /// <param name="from_col"></param>
        /// <param name="from_row"></param>
        /// <param name="to_col"></param>
        /// <param name="to_row"></param>
        /// <returns></returns>
        protected List<double> iterateGridOper(Grid content, int from_col, int from_row, int to_col, int to_row)
        {

            List<double> list = new List<double>();
            foreach (UIElement item in content.Children)
            {
                if ((Grid.GetRow(item) == (from_row-1) && Grid.GetColumn(item) == (from_col-1)) || (Grid.GetRow(item) == (to_row-1) && Grid.GetColumn(item) == (to_col-1)))
                {
                    var text = (TextBox)item;
                    if (double.TryParse(text.Text, out double n))
                    {
                        list.Add(n);
                    }
                    else if(text.Text != "")
                    {
                        MessageBox.Show("Operation cant complete becuase of non numeric values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new Exception("None Numeric Value!");
                    }
                }
            }

            return list;
        }
    }

    /// <summary>
    /// Main Class used to parse the data and apply the operations
    /// </summary>
    public class mainClass : helpingClass
    {

        /// <summary>
        /// Parsing the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public operation parse(string text)
        {
            operation current = new operation();

            //if text is not empty
            if (text != "")
            {
                //if text is not an integer
                if (!int.TryParse(text, out int n))
                {
                    //if text starts with =
                    if (text[0] == '=')
                    {
                        text = text.Replace("=", String.Empty);

                        var splited = text.Split(' '); //split using the space

                        if (splited.Length == 2) //if splited result in two
                        {
                            current.oper = ConvertOperationCode(splited[0]);

                            var row_column = splited[1].Split(':'); //split result using :

                            if (row_column.Length == 2) //if split result is not true then there is a format error
                            {
                                current.from_col = convertAlphabetToInt(row_column[0][0]);
                                current.from_row = convertToInteger(row_column[0].Substring(1));

                                current.to_col = convertAlphabetToInt(row_column[1][0]);
                                current.to_row = convertToInteger(row_column[1].Substring(1));
                            }
                            else
                            {
                                throw new Exception("Invalid Format!");
                            }

                        }
                        else if (splited.Length == 1) // for A1*A2
                        {
                            //A1*B2
                            if (splited[0].Length == 5)
                            {
                                current.oper = ConvertOperatorCode(splited[0][2]);

                                splitOperator(splited[0], splited[0][2],out current.from_col, out current.from_row,out current.to_col,out current.to_row);
                            }
                            else if(splited[0].Length == 6) //A11*B2
                            {
                                try
                                {
                                    current.oper = ConvertOperatorCode(splited[0][2]);
                                    splitOperator(splited[0], splited[0][2], out current.from_col, out current.from_row, out current.to_col, out current.to_row);
                                }
                                catch(Exception ex)
                                {
                                    current.oper = ConvertOperatorCode(splited[0][3]);
                                    splitOperator(splited[0], splited[0][3], out current.from_col, out current.from_row, out current.to_col, out current.to_row);
                                }
                            }
                            else if(splited[0].Length == 7) //A11*B22
                            {
                                current.oper = ConvertOperatorCode(splited[0][3]);
                                splitOperator(splited[0], splited[0][3], out current.from_col, out current.from_row, out current.to_col, out current.to_row);
                            }
                            else
                            {
                                throw new Exception("Invalid Format!");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Format!");
                        }
                    }
                }

            }

            return current;
        }
        /// <summary>
        /// Parser to parse for graphs
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public operation parseGraph(string text)
        {
            var splited = text.Split(':');

            if(splited.Length == 2)
            {
                operation oper = new operation();

                oper.from_col = convertAlphabetToInt(splited[0][0]);
                splited[0] = splited[0].Remove(0,1);
                oper.from_row = convertToInteger(splited[0]);

                oper.to_col = convertAlphabetToInt(splited[1][0]);
                splited[1] = splited[1].Remove(0, 1);
                oper.to_row = convertToInteger(splited[1]);

                return oper;
            }
            else
            {
                throw new Exception("Invalid Graph");
            }
        }

        /// <summary>
        /// Acctualy format the data for graph purpose
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="oper"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public List<double> applyGraph(Grid grid, operation oper, out List<string> labels)
        {
            getRowColumn(oper, out int from_row, out int from_col, out int to_row, out int to_col);
            return iterateGrid(grid, from_col, from_row, to_col, to_row, out labels);
        }

        /// <summary>
        /// Appliying the functions basis on operation/operator int code
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public double applyOperation(operation oper, Grid grid)
        {
            List<double> content = null;
            if (oper.oper < 5)
            {
                getRowColumn(oper, out int from_row, out int from_col, out int to_row, out int to_col);
                content = iterateGrid(grid,from_col,from_row,to_col,to_row);
            }
            else
            {
                content = iterateGridOper(grid, oper.from_col, oper.from_row,oper.to_col,oper.to_row);
            }

            switch (oper.oper)
            {
                case 1:
                    return Mean(content);
                case 2:
                    return Mode(content);
                case 3:
                    return Median(content);
                case 4:
                    return Sum(content);
                case 5:
                    return Multiply(content);
                case 6:
                    return Devide(content);
                case 7:
                    return Add(content);
                case 8:
                    return Subtract(content);
            }

            return -1;
        }

        //Defined operaions and operator

        public double Mean(List<double> content)
        {
            double mean = 0;

            foreach(var num in content)
            {
                mean = mean + num;
            }

            return (mean / content.Count);
        }

        public double Sum(List<double> content)
        {
            double sum = 0;

            foreach (var num in content)
            {
                sum = sum + num;
            }

            return sum;
        }

        public double Median(List<double> content)
        {
            content.Sort();

            int len = content.Count;

            if(len%2 != 0)
            {
                return content[len / 2];
            }

            return (content[(len - 1) / 2] + content[len / 2]) / 2.0;
        }

        public double Mode(List<double> content)
        {
            return (double)content.GroupBy(x => x).OrderByDescending(x => x.Count()).ThenBy(x => x.Key).Select(x => (int?)x.Key).FirstOrDefault();
        }

        public double Multiply(List<double>  content)
        {
            return content[0] * content[1];
        }

        public double Add(List<double> content)
        {
            return content[0] + content[1];
        }

        public double Devide(List<double> content)
        {
            return content[0] / content[1];
        }

        public double Subtract(List<double> content)
        {
            return content[0] - content[1];
        }

        //save the grid
        public string save(Grid grid)
        {
            string text = null;
            foreach (var item in grid.Children)
            {
                TextBox textbox = (TextBox)item;
                text += textbox.Text + ';';
            }

            return text;
        }

        public void load(Grid grid, string data)
        {
            var splited = data.Split(';');
            int i = 0;

            foreach(var item in grid.Children)
            {
                TextBox textbox = (TextBox)item;
                textbox.Text = splited[i];
                i++;
            }
        }

        public void newGrid(Grid grid)
        {
            foreach (var item in grid.Children)
            {
                TextBox textbox = (TextBox)item;
                textbox.Text = "";
            }
        }
    }
}
