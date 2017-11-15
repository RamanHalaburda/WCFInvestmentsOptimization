using System;
using System.Collections.Generic;

namespace Bionic
{
    public class Bionic
    {
        List<element> points;
        int flag = 0, size;

        int topLimit = 0, bottomLimit = 0;
        int countPopulation = 0;
        int dimPopulation = 0;

        //List<int> res;
        element answer;
        private void button1_Click(object sender, EventArgs e)
        {
            bool generate_new_flag = false;
            //bool func = false;
            //if (checkBox1.Checked)
            //{
            //    func = true;
            //}

            //int population = countPopulation;
            //size = int.Parse(textBox4.Text);
            List<element> mas = new List<element>(size);
            points = new List<element>(size * countPopulation);
            List<element> buf = new List<element>(size);
            mas = generate(size);
            for (int i = 0; i < countPopulation; i++)
            {
                addPoints(mas);
                if (buf.Count != 0)
                    buf.Clear();
                int kol = 0, flag = 0;
                for (int j = 0; j < size; j++)
                {
                    if (mas[j].elite)
                    {
                        buf.Add(mas[j]);
                        kol++;
                    }
                }
                mas.Clear();
                for (int j = 0; j < size; j++)
                {
                    double x1 = 0, x2 = 0;
                    if (flag == kol - 1)
                    {
                        flag = 0;
                        if (generate_new_flag)
                            generate_new_flag = false;
                        else
                            generate_new_flag = true;
                    }

                    if (generate_new_flag)
                        x2 = buf[flag].x2;
                    else
                        x1 = buf[flag].x1;
                    flag++;
                    if (flag == kol)
                    {
                        flag = 0;
                        if (generate_new_flag)
                            x2 = buf[flag].x2;
                        else
                            x1 = buf[flag].x1;
                        if (generate_new_flag)
                            generate_new_flag = false;
                        else
                            generate_new_flag = true;
                    }
                    else
                    {
                        if (generate_new_flag)
                            x1 = buf[flag].x1;
                        else
                            x2 = buf[flag].x2;
                    }
                    mas.Add(genereteNewElement(x1, x2));
                }
            }
            addPoints(mas);
            //label5.Text = find_best(mas).y.ToString() + ";" + find_best(mas, func).x1.ToString() + ";" + find_best(mas, func).x2.ToString();
            answer = find_best(mas);
            //button2_Click(new object(), new EventArgs());
        }
        private element find_best(List<element> mass)
        {
            double rez = 1000000;
            element rezult = new element(0, 0);
            String str = "";
            
                rez = 0;
                for (int i = 0; i < countPopulation; i++)
                {
                    str += mass[i].y.ToString() + ";";
                    if (mass[i].y > rez)
                    {
                        rez = mass[i].y;
                        rezult = mass[i];
                    }
                }
            /*
            else
            {
                for (int i = 0; i < countPopulation; i++)
                {
                    str += mass[i].y.ToString() + ";";
                    if (mass[i].y < rez)
                    {
                        rez = mass[i].y;
                        rezult = mass[i];
                    }
                }
            }
            */
            //label2.Text = rez.ToString();
            return rezult;
        }
        private List<element> generate(int size)
        {
            List<element> mas = new List<element>(size);
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {

                element elem = new element((double)rand.Next(bottomLimit * 100, topLimit * 100) / 100, (double)rand.Next(bottomLimit * 100, topLimit * 100) / 100);
                mas.Add(elem);
            }

            return mas;
        }
        private element genereteNewElement(double x1, double x2)
        {
            Random rand = new Random();
            if (rand.Next(0, 5) == 1)
            {
                x1 = x1 * 1.2;
                x2 = x2 * 0.8;
            }
            return new element(x1, x2);
        }
        private void addPoints(List<element> mass)
        {
            for (int i = 0; i < mass.Count; i++)
            {
                points.Add(mass[i]);
            }
        }
        //private void addToChart(element el)
        //{
        //    chart1.Series["Точки"].Points.AddXY(el.x1, el.x2);
        //    chart1.Series["Значение"].Points.AddXY(el.y, el.y);
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    chart1.Series["Точки"].Points.Clear();
        //    chart1.Series["Значение"].Points.Clear();
        //    flag++;
        //    if (flag == 1)
        //        button3.Enabled = true;

        //    if (flag == size)
        //        button2.Enabled = false;
        //    for (int i = (flag - 1) * size; i < flag * size; i++)
        //    {
        //        addToChart(points[i]);
        //    }

        //    label2.Text = flag.ToString();
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    chart1.Series["Точки"].Points.Clear();
        //    chart1.Series["Значение"].Points.Clear();
        //    flag--;
        //    if (flag == 0)
        //        button3.Enabled = false;
        //    if (flag == size - 1)
        //        button2.Enabled = true;
        //    for (int i = (flag) * size; i < flag * size + size; i++)
        //    {
        //        addToChart(points[i]);
        //    }

        //    label2.Text = flag.ToString();
        //}
    }

    public class element
    {
        public double x1, x2, y;
        public bool elite;
        public element(double x1, double x2)
        {
            this.x1 = x1;
            this.x2 = x2;
            //this.func = func;
            this.elite = if_elite();
            this.calculate();
        }
        private void calculate()
        {
            
                this.y = (double)(1.5 * Math.Pow(this.x1, 2) * Math.Exp(1 - Math.Pow(this.x1, 2) - 20.25 * (Math.Pow(this.x1 - this.x2, 2))) + Math.Pow(0.5 * this.x1 - 0.5, 4) * Math.Pow(this.x2 - 1, 4) * Math.Exp(2 - Math.Pow(0.5 * this.x1 - 0.5, 4) - Math.Pow(this.x2 - 1, 4)));
            /*
                this.y = (double)(-6 * x1 + 2 * Math.Pow(x2, 2) - 2 * x1 * x2 + 2 * Math.Pow(x2, 2));
            */
        }
        private bool if_elite()
        {
            bool result = true;
            
                if (this.x1 < 0)
                    result = false;
                if (this.x2 > 4)
                    result = false;
                if (Math.Pow(this.x1 - 2.2, 2) + Math.Pow(this.x2 - 1.2, 2) > 2.25)
                    result = false;
                if (Math.Pow((this.x1 - 2) / 1.2, 2) + Math.Pow(this.x2 / 2, 2) < 1)
                    result = false;
            /*
            else
            {
                if ((this.x1 + this.x2) > 2)
                {
                    result = false;
                }
                if (this.x2 < 0)
                {
                    result = false;
                }
                if (this.x1 < 0)
                {
                    result = false;
                }
            }
            */
            return result;
        }
    }
}
