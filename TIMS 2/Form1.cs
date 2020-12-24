using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;



namespace TIMS_2
{


    public partial class Form1 : Form
    {
        Random rd;
        public const double NegativeInfinity = Double.NegativeInfinity;
        static int factorial(int number)
        {
            int fact;
            fact = 1;
            for (int i = 1; i <= number; i++)
            {
                fact = fact * i;
            }
            return fact;
        }
        static int comb(int m, int n)
        {
            if (m == 0 || n - m == 0)
            {
                return 1;
            }
            else
            {
                int c = factorial(n) / (factorial(n - m) * factorial(m));
                return c;
            }
        }
        public Form1()
        {
            InitializeComponent();
            rd = new Random();

        }

        private void fromlav_Click(object sender, EventArgs e)
        {

        }

        private void generate_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            int number = Convert.ToInt32(count.Value);
            int[] Value = new int[number];

            if (change.Checked)
            {
                int element = 0;
                int t = 0;
                for (int i = 0; i < Convert.ToInt32(count.Value); ++i)
                {
                    t = 0;
                    for (int j = 0; j < Convert.ToInt32(to.Value)+1; ++j)
                    {
                        element = rd.Next(0, 2);
                        if (element == 1)
                        {
                            t += 1;
                        }
                    }
                    Value[i] = t;
                    textBox1.AppendText(t.ToString() + " ");
                }
            }

            else
            {
                int n;
                for (int i = 0; i < number; ++i)
                {
                    n = rd.Next(Convert.ToInt32(0), Convert.ToInt32(Convert.ToInt32(to.Value)+1));
                    Value[i] = n;
                    textBox1.AppendText(n.ToString() + " ");


                }
            }
         
            Dictionary<int, int> dictValue = new Dictionary<int, int>();
            for (int i = 0; i < Value.Length; i++)
            {


                try
                {
                    dictValue.Add(Value[i], 1);
                }
                catch
                {
                    dictValue[Value[i]] += 1;
                }


            }
            dataGridView1.Rows.Clear();


            for (int i = Convert.ToInt32(0); i < to.Value+1; i++)
            {


                try
                {
                    dataGridView1.Rows.Add(i, dictValue[i]);
                }
                catch
                {
                    //dataGridView1.Rows.Add(i, 0);
                }


            }


        }
        class Binomial
        {
            public int xi { get; set; }
            public int ni { get; set; }

            public Binomial(int Xi, int Ni)
            {
                xi = Xi;
                ni = Ni;
            }
            public override string ToString()
            {
                return "xi = " + xi.ToString() + "  ni= " + ni.ToString() + '\n';
            }
        }
        class Normal
        {
            public double xi { get; set; }
            public double xi1 { get; set; }
            public int ni { get; set; }
            public Normal(double Xi, double Xi1, int Ni)
            {
                xi = Xi;
                xi1 = Xi1;
                ni = Ni;

            }
            public override string ToString()
            {
                return $"[{xi} ; {xi1}]    ni = {ni}\n";
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Binomial> binomCollection = new List<Binomial>();



            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                Binomial b = new Binomial(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value), Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value));
                binomCollection.Add(b);

            }

            double alp = Convert.ToDouble(alpNumeric.Value);
            double xser = 0;
            int N = binomCollection[binomCollection.Count - 1].xi;
            double n = 0;

            for (int i = 0; i < binomCollection.Count; i++)
            {
                xser += binomCollection[i].xi * binomCollection[i].ni;
               n += binomCollection[i].ni;
            }


            xser /= n;
            double p = xser / N;
            labelp.Text = p.ToString();
            List<double> pi = new List<double>();
            double a = 0;
            List<int> mi = new List<int>();
            foreach (Binomial d in binomCollection)
            {
                mi.Add(d.ni);
            }
            double q = 1 - p;
            foreach (Binomial c in binomCollection)
            {
                a = comb(c.xi, N) * Math.Pow(p, c.xi) * Math.Pow(q, (N - c.xi));
                pi.Add(a);


            }
            DataTable dt = new DataTable();
            DataRow r = dt.NewRow();

            DataRow r1 = dt.NewRow();
            DataRow r2 = dt.NewRow();
            for (int i = 0; i < pi.Count; i++)
            {
                dt.Columns.Add(binomCollection[i].xi.ToString());

                r1[i] = mi[i];
                r[i] = Math.Round(pi[i],5);
                r2[i] =Math.Round(pi[i] * n,5);
            }

            dt.Rows.Add(r1);
            dt.Rows.Add(r);
            dt.Rows.Add(r2);
            dataGridView2.DataSource = dt;
            dataGridView2.Rows[0].HeaderCell.Value = "mi";
            dataGridView2.Rows[2].HeaderCell.Value = "pi*n";
            dataGridView2.Rows[1].HeaderCell.Value = "pi";

            
       
          

                int index = 0;
                for (int i = 0; i < binomCollection.Count; i++)
                {
                    
                    if (binomCollection[i].ni < 5)
                    {

                        if (i == binomCollection.Count - 1)
                        {
                            mi[index - 1] = mi[index] + mi[index - 1];
                            mi.RemoveAt(index);
                            pi[index - 1] = pi[index] + pi[index - 1];
                            pi.RemoveAt(index);
                            index -= 1;
                        }
                        else
                        {
                            mi[index + 1] = mi[index] + mi[index + 1];
                            mi.RemoveAt(index);
                            pi[ + 1] = pi[index] + pi[index + 1];
                            pi.RemoveAt(index);
                            index -= 1;

                        }
                    }
                    index += 1;
                }
            
         


            for (int i = 0; i < pi.Count; i++)
            {
                if (pi[i] * n < 10)
                {

                    if (i == pi.Count - 1)
                    {
                        mi[i - 1] = mi[i] + mi[i - 1];
                        mi.RemoveAt(i);
                        pi[i - 1] = pi[i] + pi[i - 1];
                        pi.RemoveAt(i);
                    }
                    else
                    {
                        mi[i + 1] = mi[i] + mi[i + 1];
                        mi.RemoveAt(i);
                        pi[i + 1] = pi[i] + pi[i + 1];
                        pi.RemoveAt(i);
                        if (pi[i] * n < 10)
                        {
                            i -= 1;
                        }
                    }
                }
            }
            //lab.Text = "таблиця після об'єднання колонок, якщо n*pi<10 або mi<10";
            DataTable dt2 = new DataTable();
            DataRow r3 = dt2.NewRow();
            DataRow r4 = dt2.NewRow();
            DataRow r5 = dt2.NewRow();
            string aqw = " ";
            for (int i = 0; i < mi.Count; ++i)
            {
                
                dt2.Columns.Add(aqw);
                r3[i] = mi[i];
                r4[i] = Math.Round(pi[i],5);
                r5[i] = Math.Round(pi[i] * n, 5);
                aqw += " ";
            }

            dt2.Rows.Add(r3);
            dt2.Rows.Add(r4);
            dt2.Rows.Add(r5);
            dataGridView3.DataSource = dt2;
            dataGridView3.Rows[0].HeaderCell.Value = "mi";
            dataGridView3.Rows[1].HeaderCell.Value = "pi";
            dataGridView3.Rows[2].HeaderCell.Value = "pi*n";
            double Femp = 0;
            for (int i = 0; i < mi.Count; i++)
            {
                double temp = mi[i] - (pi[i] * n);
                Femp += Math.Pow(temp, 2) / (pi[i] * n);
            }
            labelxi.Text = Femp.ToString();
            int df = mi.Count - 1 - 1;
            labeldf.Text = df.ToString();
            try
            {
                double Fcrt = ChiSquared.InvCDF(df, 1 - alp);


                labelsicrit.Text = Fcrt.ToString();

                if (Fcrt < Femp)
                {
                    visnovok1.Text = "Оскільки Хі критичне < критерію Пірсона, то гіпотезу відхиляємо";

                }
                else
                {
                    visnovok1.Text = "Оскільки Хі критичне > критерію Пірсона, то гіпотезу приймаємо";
                }
            }
            catch
            {
                MessageBox.Show("кількість ступенів вільності = "+df.ToString());
                this.Close();
                  
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabbinom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelxi_Click(object sender, EventArgs e)
        {

        }

        private void labelsicrit_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mylabel:
           
            textBox2.Clear();
            int number = Convert.ToInt32(Count2.Value);
            double[] Value = new double[number];
            double n;
            for (int i = 0; i < number; ++i)
            {
                //n = rd.Next(Convert.ToInt32(from2.Value), Convert.ToInt32(to2.Value)*1000)/1000.0;
                n = rd.NextDouble() * (Convert.ToDouble(to2.Value) - Convert.ToDouble(from2.Value)+1) + Convert.ToDouble(from2.Value);
                Value[i] = n;
                textBox2.AppendText(Math.Round(n,4) + "  |  ");
            }
            double x = Math.Log(Convert.ToInt32(Count2.Value),2);
          
            x = Math.Floor(x);
            x += 1;
          

            label123.Text = x.ToString();
            Array.Sort(Value);
            double interval_count = (Convert.ToDouble(to2.Value) - Convert.ToDouble(from2.Value) )/ x;
            double start = Convert.ToDouble(from2.Value);
            double end = start + interval_count;
            int c = 0;
            double[] intervals = new double[Convert.ToInt32(x)];
            double[] countForIntervals = new double[Convert.ToInt32(x)];
            dataGridView4.Rows.Clear();
            for (int i = 0; i < Value.Length; ++i)
            {
                if(Value[i]>=start && Value[i] < end)
                {
                    c += 1;
                }
                if (Value[i] >= end )
                {
                    if (i == Value.Length - 1)
                    {
                        c += 1;
                    }
                  
                    intervals.Append(Convert.ToDouble(Math.Round((start+end)/2,2)));
                    countForIntervals.Append(c);
                    dataGridView4.Rows.Add(Convert.ToString(Math.Round(start,2) + "  " + Math.Round(end,2)), c);
                    start += interval_count;
                    end += interval_count;
                    c = 1;

                }
            }
            
            if (dataGridView4.Rows.Count-1 < x)
            {
                goto mylabel;
            }



        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Normal> normColl = new List<Normal>();
           
            string[] temp5 = new string[2];
            string temp2 = "";
            for (int i = 0; i < dataGridView4.Rows.Count - 1; i++)
            {
                temp2 = dataGridView4.Rows[i].Cells[0].Value.ToString();
                temp5 = temp2.Split(new char[] {' '});
         
        
                Normal b = new Normal(Convert.ToDouble(temp5[0]), Convert.ToDouble(temp5[2]), Convert.ToInt32(dataGridView4.Rows[i].Cells[1].Value));
                normColl.Add(b);
               

            }
           
         
            double alp2 = Convert.ToDouble(alpnumeric2.Value);
            double xser2 = 0;
            double n2 = 0;
            double ser = (normColl[0].xi1 - normColl[0].xi) / 2.0;
            for (int i = 0; i < normColl.Count; i++)
            {
                xser2 += (normColl[i].xi + ser) * normColl[i].ni;
                n2 += normColl[i].ni;
            }
            xser2 /= n2;
            double A = xser2;
            
            double D = 0;
            for (int i = 0; i < normColl.Count; i++)
            {
                double qwe = normColl[i].xi + ser - xser2;
                D += Math.Pow(qwe, 2) * normColl[i].ni;
            }
            D /= n2;
            double sigma = Math.Sqrt(D);
           
            ssqrt.Text = Math.Round(D,4).ToString();
            labels.Text = Math.Round(sigma,4).ToString();
          
            List<double> Pi = new List<double>();
            for (int i = 0; i < normColl.Count; i++)
            {
                double result = 0;
                if (i == 0)
                {
                    double k = MathNet.Numerics.Distributions.Normal.CDF(A, sigma, -100);
                    double k1 = MathNet.Numerics.Distributions.Normal.CDF(A, sigma, normColl[i].xi1);
                    result = k1 - k;
                }
                if (i == normColl.Count - 1)
                {
                    double sum = 0;
                    foreach (double pp in Pi)
                    {
                        sum += pp;
                    }
                    result = 1 - sum;
                }
                else
                {
                    double k = MathNet.Numerics.Distributions.Normal.CDF(A, sigma, normColl[i].xi);
                    double k1 = MathNet.Numerics.Distributions.Normal.CDF(A, sigma, normColl[i].xi1);
                    result = k1 - k;
                }

                Pi.Add(result);

            }

            foreach (double pr in Pi)
            {
                Console.Write(pr + "  ");
            }
      



            List<int> Mi = new List<int>();
            foreach (Normal nor in normColl)
            {
                Mi.Add(nor.ni);
            }
            DataTable dt3 = new DataTable();
            DataRow r6 = dt3.NewRow();
            DataRow r7 = dt3.NewRow();
            DataRow r8 = dt3.NewRow();
            string aqwe = " ";
            for (int i = 0; i < Mi.Count; ++i)
            {
                if (i == Mi.Count - 1)
                {
                    dt3.Columns.Add("[" + normColl[i].xi.ToString() + ";" + normColl[i].xi1.ToString() + "]");
                }
                else
                {
                    dt3.Columns.Add("[" + normColl[i].xi.ToString() + ";" + normColl[i].xi1.ToString() + ")");
                }

                
          
                r6[i] = Mi[i];
                r7[i] = Math.Round(Pi[i], 5);
                r8[i] = Math.Round(Pi[i] * n2, 5);
                aqwe += " ";
            }

            dt3.Rows.Add(r6);
            dt3.Rows.Add(r7);
            dt3.Rows.Add(r8);
            dataGridView5.DataSource = dt3;
            dataGridView5.Rows[0].HeaderCell.Value = "mi";
            dataGridView5.Rows[1].HeaderCell.Value = "pi";
            dataGridView5.Rows[2].HeaderCell.Value = "pi*n";
            int index = 0;
                for (int i = 0; i < Mi.Count; i++)
                {
                    if (normColl[i].ni < 5)
                    {
                        if (i == Mi.Count - 1)
                        {
                            Mi[index - 1] = Mi[index] + Mi[index - 1];
                            Mi.RemoveAt(index);
                            Pi[index - 1] = Pi[index] + Pi[index - 1];
                            Pi.RemoveAt(index);
                        }
                        else
                        {
                            Mi[index + 1] = Mi[index] + Mi[index + 1];
                            Mi.RemoveAt(index);
                            Pi[index + 1] = Pi[index] + Pi[index + 1];
                            Pi.RemoveAt(index);
                        index -= 1;
                        }
                    }
                index += 1;
                }
            
            for (int j = 0; j < Pi.Count; j++)
            {
                for (int i = 0; i < Pi.Count; i++)
                {
                    if (Pi[i] * n2 < 10)
                    {
                        if (i == Pi.Count - 1)
                        {
                            Mi[i - 1] = Mi[i] + Mi[i - 1];
                            Mi.RemoveAt(i);
                            Pi[i - 1] = Pi[i] + Pi[i - 1];
                            Pi.RemoveAt(i);
                        }
                        else
                        {
                            Mi[i + 1] = Mi[i] + Mi[i + 1];
                            Mi.RemoveAt(i);
                            Pi[i + 1] = Pi[i] + Pi[i + 1];
                            Pi.RemoveAt(i);
                            if (Pi[i] * n2 < 10)
                            {
                                i -= 1;
                            }
                        }
                    }
                }
            }

            DataTable dt4 = new DataTable();
            DataRow r9 = dt4.NewRow();
            DataRow r10 = dt4.NewRow();
            DataRow r11 = dt4.NewRow();
            string aqwer = " ";
            for (int i = 0; i < Mi.Count; ++i)
            {

                dt4.Columns.Add(aqwer);

                r9[i] = Mi[i];
                r10[i] = Math.Round(Pi[i], 5);
                r11[i] = Math.Round(Pi[i] * n2, 5);
                aqwer += " ";
            }

            dt4.Rows.Add(r9);
            dt4.Rows.Add(r10);
            dt4.Rows.Add(r11);
            dataGridView6.DataSource = dt4;
            dataGridView6.Rows[0].HeaderCell.Value = "mi";
            dataGridView6.Rows[1].HeaderCell.Value = "pi";
            dataGridView6.Rows[2].HeaderCell.Value = "pi*n";
           
            double Femp2 = 0;
            for (int i = 0; i < Mi.Count; i++)
            {
                double temp = Mi[i] - (Pi[i] * n2);
                Femp2 += Math.Pow(temp, 2) / (Pi[i] * n2);
            }

            xiaqrt2.Text = Femp2.ToString();
            int df2 = Mi.Count - 1 - 2;
            labelalp2.Text = df2.ToString();
            try
            {
                double Fcrt2 = ChiSquared.InvCDF(df2, 1 - alp2);
                
                xicrit2.Text = Fcrt2.ToString();
                if (Fcrt2 < Femp2)
                {
                    visnovok123.Text = "Оскільки Хі критичне < критерію Пірсона, то гіпотезу відхиляємо";
                }
                else
                {
                    visnovok123.Text="Оскільки Хі критичне > критерію Пірсона, то гіпотезу приймаємо";
                }
            }
            catch 
            {
                MessageBox.Show("Кількість ступенів вільності= "+df2.ToString());
                this.Close();
            }
        }



        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void to2_ValueChanged(object sender, EventArgs e)
        {

        }
        bool isChecked = false;
        private void change_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = change.Checked;
       
        }

        private void change_Click(object sender, EventArgs e)
        {
            if (change.Checked && !isChecked)
                change.Checked = false;
            else
            {
                change.Checked = true;
                isChecked = false;
            }
        }
    }
}
