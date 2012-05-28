using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace cnvrt
{
    class Sintactic
    {
        private enum define
        {
            AND = 1,
            ASM,
            ARRAY,
            BEGIN,
            CASE,
            CONST,
            CONSTRUCTOR,
            DESTRUCTOR,
            DIV,
            DO,
            DOWNTO,
            ELSE,
            END,
            EXPORT,
            FILE,
            FOR,
            FUNCTION,
            GOTO,
            IF,
            IMPLEMENTATION,
            IN,
            INHERITED,
            INLINE,
            INTERFACE,
            LABEL,
            LIBRARY,
            MOD,
            NIL,
            NOT,
            OBJECT,
            OF,
            OR,
            PACKED,
            PROCEDURE,
            PROGRAM,
            RECORD,
            REPEAT,
            SET,
            SHL,
            SHR,
            STRING,
            THEN,
            TO,
            TYPE,
            UNIT,
            UNTIL,
            USES,
            VAR,
            WHILE,
            WITH,
            XOR,
            INTEGER,
            LONGINT,
            REAL,
            CHAR,
            STEP,
            READ,
            WRITE,
            OTHERWISE,
            IDENTIFIER,
            CHARACTER,
            PLUS,
            MINUS,
            MUL,
            DIVIZARE,
            ASSIGN,
            EQUAL,
            NOTEQUAL,
            MAIMICSAUEGAL,
            MAIMARESAUEGAL,
            PARL,
            PARR,
            ACOLL,
            ACOLR,
            PARDL,
            PARDR,
            PUNCT,
            VIRGULA,
            PUNCTSIVIRGULA,
            DOUAOUNCTE,
            DOMENIU,
            MAIMIC,
            MAIMARE,
            SIRCARACTERE,
            NRINTEGER,
            NRREAL,
            AT,
            POINTER,
            DOLLAR,
            COMENTARIU,
            GHILIMELE,
            GHILIMELESIMPLE,
            EXCLAM,
            DIEZ,
            PROCENT,
            SI,
            INTREBARE,
            BACKSLASH,
            SAU,
            NUMEPROGRAM
        };
        private Lexical lex;
        private TextBox text;
        private string[] codPascal;
        private Atom curent_atom = new Atom(0, null, 0);
        private Atom anterior_atom = new Atom(0, null, 0);
        private Atom urmator_atom = new Atom(0, null, 0);
        bool eroare = false;
        string temp = "";
        int tabulator = 0;
        bool eroare_f = false;
        string tip_variabila = null;
        string error_display = "";
        string pr = "secund";
        System.Collections.Hashtable lista_variabile = new System.Collections.Hashtable();
        /// <summary>
        /// constructorul clasei sintactic, creaza un analizor sintactic
        /// </summary>
        /// <param name="lista">codul pascal inpartit pe linii</param>
        /// <param name="text">obiect de tip textbox in care se va afisa codul C care se produce in urma compilarii</param>
        public Sintactic(string[] lista, TextBox text)
        {
            this.lex = new Lexical(lista);
            this.text = text;
            this.text.Text = "";
            this.codPascal = lista;
            this.lex.Analizare();
            urmator_atom = lex.UrmatorAtom();
            
            lista_variabile.Add("integer", "tipvariabila");
            lista_variabile.Add("char", "tipvariabila");
            lista_variabile.Add("real", "tipvariabila");
            lista_variabile.Add("longint", "tipvariabila");
            lista_variabile.Add("record", "tipvariabila");
        }
        /// <summary>
        /// functie pentru translatarea atomilor din pascal in C
        /// </summary>
        /// <param name="atom">atomul de translatat</param>
        /// <returns>atomul rezultat in urma translatarii</returns>
        private string TranslatareAtom(Atom atom)
        {
            if (atom.valoare.CompareTo("begin") == 0)
            {
                tabulator++;
                return "{\r\n";
            }
            else if (atom.valoare.CompareTo("end") == 0)
            {
                return "}\r\n";
            }
            else if (atom.valoare.CompareTo("uses") == 0) return "#include";
            else if (atom.valoare.CompareTo("if") == 0) return "if";
            else if (atom.valoare.CompareTo("then") == 0) return " ";
            else if (atom.valoare.CompareTo("else") == 0) return "else";
            else if (atom.valoare.CompareTo("while") == 0) return "while";
            else if (atom.valoare.CompareTo("do") == 0) return " ";
            else if (atom.valoare.CompareTo("repeat") == 0) return "do \r\n";
            else if (atom.valoare.CompareTo("until") == 0) return "}; while!";
            else if (atom.valoare.CompareTo("procedure") == 0) return "void ";
            else if (atom.valoare.CompareTo("function") == 0) return " ";
            else if (atom.valoare.CompareTo("boolean") == 0) return "int";
            else if (atom.valoare.CompareTo("integer") == 0) return "int";
            else if (atom.valoare.CompareTo("longint") == 0) return "long int";
            else if (atom.valoare.CompareTo("real") == 0) return "float";
            else if (atom.valoare.CompareTo("word") == 0) return "unsigned int";
            else if (atom.valoare.CompareTo("byte") == 0) return "unsigned char";
            else if (atom.valoare.CompareTo("false") == 0) return "0";
            else if (atom.valoare.CompareTo("true") == 0) return "1";
            else if (atom.valoare.CompareTo("and") == 0) return " && ";
            else if (atom.valoare.CompareTo("or") == 0) return " || ";
            else if (atom.valoare.CompareTo("not") == 0) return "!";
            else if (atom.valoare.CompareTo(",") == 0) return ", ";
            else if (atom.valoare.CompareTo(".") == 0) return "";
            else if (atom.valoare.CompareTo(";") == 0) return ";\r\n";
            else if (atom.valoare.CompareTo(":=") == 0) return " = ";
            else if (atom.valoare.CompareTo(":") == 0) return " ";
            else if (atom.valoare.CompareTo("+") == 0) return " + ";
            else if (atom.valoare.CompareTo("-") == 0) return " - ";
            else if (atom.valoare.CompareTo("*") == 0) return " * ";
            else if (atom.valoare.CompareTo("/") == 0) return " / ";
            else if (atom.valoare.CompareTo("=") == 0) return " == ";
            else if (atom.valoare.CompareTo("<>") == 0) return " != ";
            else if (atom.valoare.CompareTo("><") == 0) return " != ";
            else if (atom.valoare.CompareTo("{") == 0) return "/* \r\n ";
            else if (atom.valoare.CompareTo("}") == 0) return "*/ \r\n ";
            else if (atom.valoare.CompareTo("(") == 0) return "(";
            else if (atom.valoare.CompareTo(")") == 0) return ") ";
            else if (atom.valoare.CompareTo("nil") == 0) return "NULL";
            else if (atom.valoare.CompareTo("case") == 0) return "switch (";
            else if (atom.valoare.CompareTo("of") == 0) return ") {";
            else if (atom.valoare.CompareTo("record") == 0) return "struct {";
            else if (atom.valoare.CompareTo("pointer") == 0) return "void *";
            else if (atom.valoare.CompareTo("private") == 0) return "private:";
            else if (atom.valoare.CompareTo("program") == 0)
            {
                return "\r\n//";
            }
            else if (atom.valoare.CompareTo("var") == 0) return "\r\n";
            else if (atom.valoare.CompareTo("write") == 0) return "printf";
            else if (atom.valoare.CompareTo("scan") == 0) return "scanf";
            else if (atom.valoare.CompareTo("mod") == 0) return "%";
            else if (atom.valoare.CompareTo("div") == 0) return "/";
            else if (atom.valoare.CompareTo("const") == 0) return "const";
            else return atom.valoare;
        }
        /// <summary>
        /// Functia cere urmatorul atom de la analizorul lexical
        /// </summary>
        private void NextAtom()
        {
            anterior_atom.indice = curent_atom.indice;
            anterior_atom.valoare = curent_atom.valoare;
            anterior_atom.linie = curent_atom.linie;
            curent_atom.linie = urmator_atom.linie;
            curent_atom.indice = urmator_atom.indice;
            curent_atom.valoare = urmator_atom.valoare;
            urmator_atom = lex.UrmatorAtom();
        }
        /// <summary>
        /// functia adauga la codul C
        /// </summary>
        /// <param name="deAdaugat">sirul de caractere care se va adauga la codul C</param>
        private void AdaugareInCod(string deAdaugat)
        {
            if (urmator_atom != null)
            {
                if ((int)urmator_atom.indice != (int)define.END)
                {
                    if (eroare == false)
                    {
                        text.Text += deAdaugat;
                    }
                    else
                    {
                        text.Text += deAdaugat + "/* <- EROARE */";
                        eroare = false;
                    }
                    if (deAdaugat.Contains("\r\n") == true)
                    {

                        for (int i = 0; i < tabulator; i++)
                        {
                            text.Text += "\t";
                        }
                    }
                }
                else
                {
                    if ((int)curent_atom.indice == (int)define.BEGIN)
                    {
                        if (eroare == false)
                        {
                            text.Text += deAdaugat;
                        }
                        else
                        {
                            text.Text += deAdaugat + "/* <- EROARE */";
                            eroare = false;
                        }
                        if (deAdaugat.Contains("\r\n") == true)
                        {

                            for (int i = 0; i < tabulator - 1; i++)
                            {
                                text.Text += "\t";
                            }
                        }
                    }

                    else
                    {
                        if (eroare == false)
                        {
                            text.Text += deAdaugat;
                        }
                        else
                        {
                            text.Text += deAdaugat + "/* <- EROARE */ ";
                            eroare = false;
                        }
                        if (deAdaugat.Contains("\r\n") == true)
                        {

                            if ((int)urmator_atom.indice == (int)define.END)
                            {
                                tabulator--;
                                for (int i = 0; i < tabulator; i++)
                                {
                                    text.Text += "\t";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < tabulator; i++)
                                {
                                    text.Text += "\t";
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// cauta urmatorul atom de sincronizare, ca este end sau ";"
        /// </summary>
        private void AtomSincronizare()
        {
            while ((int)curent_atom.indice != (int)define.END && (int)curent_atom.indice != (int)define.PUNCTSIVIRGULA)
                NextAtom();
            AdaugareInCod("\r\n");
        }
        /// <summary>
        /// Se apeleaza in cazul unei erori si se afisa erorile la sfarsitul compilarii
        /// </summary>
        /// <param name="errorString">Eroarea care se va adauga la sirul de erori</param>
        private void Errors(string errorString)
        {
            error_display += errorString + "\r\n";
            AtomSincronizare();
            eroare_f = true;
        }
        /// <summary>
        /// incepera compilarii codului pascal
        /// </summary>
        /// <returns>true daca nu este eroare in program si false cand este o eroare grava</returns>
        public bool BeginTranslation()
        {
            NextAtom();
            if ((int)curent_atom.indice == (int)define.PROGRAM)
            {
                NextAtom();
                if ((int)curent_atom.indice == (int)define.IDENTIFIER)
                {
                    AdaugareInCod("//program:  " + TranslatareAtom(curent_atom));
                    AdaugareInCod("()");
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                    {
                        Errors("Eroare: Nu ati introdus numele programului!");
                    }
                    else
                    {
                        Errors("Eroare: Nu ati introdus numele programului!");
                    }
                    return false;
                }
                if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    Errors("Eroare: Nu ati introdus punct si virgula \"'\" de la sfarsitul liniei!");
                    eroare = true;
                    return false;

                }
            }
            else
            {
                AdaugareInCod("//Nu ati introdus numele programului");
            }

            BlocInstr();

            if ((int)curent_atom.indice != (int)define.PUNCT)
            {
                Errors("Eroare: Nu ati introdus punctul la sfarsitul programului!");
                eroare = true;
                if (error_display.Length > 1)
                {
                    MessageBox.Show(error_display, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;

            }
            else
            {
                AdaugareInCod("\r\n");
                AdaugareInCod(TranslatareAtom(curent_atom));
                if (error_display.Length > 1)
                {
                    MessageBox.Show(error_display, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return true;
            }
        }
        /// <summary>
        /// functie de verificare a blocului de instructiuni si declarare variabile
        /// </summary>
        private void BlocInstr()
        {
            while ((int)curent_atom.indice == (int)define.VAR || ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA && eroare_f==true) )
            {
                if ((int)curent_atom.indice == (int)define.VAR)
                {
                    BlocVariabile();
                }
                if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA && eroare_f == true) 
                {
                    NextAtom();
                }      
            }
            AdaugareInCod("\r\nvoid main()\r\n");
            Instructii_Begin_End();
        }
        /// <summary>
        /// functie de determinare a variabilel
        /// </summary>
        private void BlocVariabile()
        {
            if ((int)curent_atom.indice == (int)define.VAR)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                AdaugareInCod("//variabile globale\r\n");
                DeclarareVariabile();
            }
            else
            {
                AdaugareInCod("\r\n//Nu exista variabile globale");
            }
        }
        /// <summary>
        /// verificarea cazului in care exista mai multe tipuri de variabile declarate
        /// </summary>
        /// <returns>false in cazul in care exista eroare sau nu mai exista variabile</returns>
        private bool DeclarareVariabile()
        {
            if (VariabileNoi())
                DeclarareVariabile();
            return true;

        }
        /// <summary>
        /// verificarea cazului in care exista mai multe tipuri de variabile declarate
        /// </summary>
        /// <returns>false in cazul in care exista eroare sau nu mai exista variabile</returns>
        private bool VariabileNoi()
        {
            if (TipuriDeVariabile())
            {
                VariabileNoi();
                return true;
            }
            return false;
        }
        /// <summary>
        /// verificarea tipurilor de variabile
        /// </summary>
        /// <returns>false in cazul in care exista o eroare</returns>
        private bool TipuriDeVariabile()
        {
            if (ListaDeIdentificatori())
            {
                if ((int)curent_atom.indice == (int)define.DOUAOUNCTE)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    Errors("Eroare: Nu ati introdus doua puncte \":\" din linia de declararea a variabilelor");
                    eroare = true;
                }
                if ((int)curent_atom.indice == (int)define.ARRAY)
                {
                    NextAtom();
                    int inceput = 0, sfarsit = 0;
                    if ((int)curent_atom.indice == (int)define.PARDL)
                    {
                        NextAtom();
                        if ((int)curent_atom.indice == (int)define.NRINTEGER)
                        {
                            inceput = System.Convert.ToInt32(curent_atom.valoare);
                            NextAtom();
                            if ((int)curent_atom.indice == (int)define.DOMENIU)
                            {
                                NextAtom();
                                if ((int)curent_atom.indice == (int)define.NRINTEGER)
                                {
                                    sfarsit = System.Convert.ToInt32(curent_atom.valoare);
                                    NextAtom();
                                    if ((int)curent_atom.indice == (int)define.PARDR)
                                    {
                                        NextAtom();
                                        if (curent_atom.valoare.CompareTo("of") == 0)
                                        {
                                            NextAtom();
                                            TipuriVariabile();
                                            if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                                            {
                                                string[] variabile = temp.Split(' ');
                                                temp = "";
                                                int k = -1;
                                                do
                                                {
                                                    k++;
                                                    temp = variabile[k] + " ";

                                                }
                                                while (variabile[k].Length == 0);
                                                for (int i = 1; i < variabile.Length; i++)
                                                {
                                                    variabile[i] = variabile[i].Replace(",", "");
                                                    variabile[i] = variabile[i].Replace(";", "");
                                                    if (variabile[i].Length > 0)
                                                    {
                                                        if (lista_variabile.ContainsKey(variabile[i]) == true)
                                                        {
                                                            Errors("Variabila: " + variabile[i] + " , deja exista");
                                                            eroare = true;
                                                        }
                                                        else
                                                        {
                                                            string var = variabile[i];
                                                            if (inceput < sfarsit)
                                                            {
                                                                for (int j = inceput; j < sfarsit; j++)
                                                                {
                                                                    var += "[" + j.ToString() + "]";
                                                                    lista_variabile.Add(var, anterior_atom.valoare);
                                                                    var = var.Remove(var.Length - 3);
                                                                }
                                                                variabile[i] = variabile[i] + "[" + System.Convert.ToSingle(sfarsit - inceput) + "]";
                                                            }
                                                            else
                                                            {
                                                                eroare = true;
                                                                Errors("Eroare: Primul numar trebuie sa fie mai mic ca si cel de al doilea!");
                                                            }
                                                            temp += variabile[i];
                                                            if (i < variabile.Length - 1)
                                                            {
                                                                if (variabile[i].CompareTo("int") != 0)
                                                                {
                                                                    temp += ", ";
                                                                }
                                                                else
                                                                {
                                                                    temp += " ";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                AdaugareInCod(temp);
                                                temp = "";
                                                AdaugareInCod(TranslatareAtom(curent_atom));
                                                NextAtom();
                                            }
                                            else
                                            {
                                                Errors("Eroare: Nu ati introdus punct si virgula \";\" de la sfarsitul liniei!");
                                                eroare = true;
                                                AdaugareInCod(temp);
                                            }
                                        }
                                        else
                                        {
                                            eroare = true;
                                            Errors("Eroare: Nu ati introdus cuvantul cheie \"of\"!");
                                        }
                                    }
                                    else
                                    {
                                        eroare = true;
                                        Errors("Eroare: Nu ati introdus \"]\"!");
                                    }
                                }
                                else
                                {
                                    eroare = true;
                                    Errors("Eroare: Numarul trebuie sa fie de tip intreg!");
                                }
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Lipseste operatorul: \"..\"!");
                            }
                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Numarul trebuie sa fie de tip intreg!");
                        }
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare: Nu ati introdus \"[\"!");
                    }
                }
                else
                {
                    TipuriVariabile();

                    if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                    {
                        string[] variabile = temp.Split(' ');
                        temp = "";
                        int k = -1;
                        do
                        {
                            k++;
                            temp = variabile[k] + " ";

                        }
                        while (variabile[k].Length == 0);

                        for (int i = 1; i < variabile.Length; i++)
                        {
                            variabile[i] = variabile[i].Replace(",", "");
                            variabile[i] = variabile[i].Replace(";", "");
                            if (variabile[i].Length > 0)
                            {
                                if (lista_variabile.ContainsKey(variabile[i]) == true)
                                {
                                    Errors("Eroare: Variabila " + variabile[i] + " exista!");
                                    eroare = true;
                                }
                                else
                                {
                                    lista_variabile.Add(variabile[i], anterior_atom.valoare);
                                    temp += variabile[i];
                                    if (i < variabile.Length - 1)
                                    {
                                        if (variabile[i].CompareTo("int") != 0)
                                        {
                                            temp += ", ";
                                        }
                                        else
                                        {
                                            temp += " ";
                                        }
                                    }
                                }
                            }
                        }
                        AdaugareInCod(temp);
                        temp = "";
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                    }
                    else
                    {
                        Errors("Eroare: Nu ati introdus punct si virgula \";\" de la sfarsitul liniei!");
                        eroare = true;
                        AdaugareInCod(temp);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// identificarea variabilelor si a numelor care le corespund
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool ListaDeIdentificatori()
        {
            if ((int)curent_atom.indice == (int)define.IDENTIFIER)
            {
                temp += TranslatareAtom(curent_atom);
                NextAtom();
            }
            else
            {
                return false;
            }
            Identificatori();
            return true;
        }
        /// <summary>
        /// verificarea corectitudinii de declararea a variabilelor
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Identificatori()
        {
            if ((int)curent_atom.indice == (int)define.VIRGULA)
            {
                temp += TranslatareAtom(curent_atom) + " ";
                NextAtom();
                if ((int)curent_atom.indice == (int)define.IDENTIFIER)
                {
                    temp += TranslatareAtom(curent_atom);
                    NextAtom();
                    Identificatori();
                }
                else
                {
                    Errors("Eroare: Nu ati introdus numele variabile de dupa virgula \",\"!");
                    eroare = true;
                }
            }
            return true;
        }
        /// <summary>
        /// identificarea tipurilor de variabile
        /// </summary>
        /// <returns>false in cazul in care tipul de variabila declarat nu exista</returns>
        private bool TipuriVariabile()
        {
            if ((int)curent_atom.indice == (int)define.INTEGER)
            {
                temp = "int" + " " + temp;
                NextAtom();

            }
            else if ((int)curent_atom.indice == (int)define.REAL)
            {
                temp = "float" + " " + temp;
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.CHAR)
            {
                temp = "char" + " " + temp;
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.LONGINT)
            {
                temp = "long int" + " " + temp; /// long int
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.RECORD)
            {
                temp = "struct" + " " + temp; /// long int
                NextAtom();
            }
            else
            {
                Errors("Eroare: Tip de variabila neidentificata: " + curent_atom.valoare);
                NextAtom();
                eroare = true; ;
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea blocului begin-end de la inceputul si sfarsitul programului
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Instructii_Begin_End()
        {
            if ((int)curent_atom.indice == (int)define.BEGIN)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                Instructi_verificare();
                if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                if ((int)curent_atom.indice == (int)define.END)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus \"end\"!");
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// functie de tratar a instructiunilor
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Instructi_verificare()
        {
            if (Instructii() == false)
            {
                eroare = true;
                Errors("Eroare: Instructie inexistenta!");
                return false;
            }
            Instructii_repetare();
            return true;
        }
        /// <summary>
        /// verificare ciclica daca exista mai multe instructii in cadrul blocului de program
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Instructii_repetare()
        {
            if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA || (int)anterior_atom.indice == (int)define.ACOLR)
            {
                if ((int)curent_atom.indice == (int)define.PUNCTSIVIRGULA)
                {
                    if ((int)anterior_atom.indice != (int)define.END)
                    {
                        AdaugareInCod(TranslatareAtom(curent_atom));
                    }
                    NextAtom();
                }
                if (Instructii() == false)
                {
                    return false;
                }
                return Instructii_repetare();
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// identificarea instructiilor din cadrul programului
        /// </summary>
        /// <returns>false in cazul unei instructiuni neidentificate sau a unei erori</returns>
        private bool Instructii()
        {
            if (Asignare())
            {
                return true;
            }
            else if (If())
            {
                return true;
            }
            else if (While())
            {
                return true;
            }
            else if (For())
            {
                return true;
            }
            else if (Repeat())
            {
                return true;
            }
            else if (Instructii_Begin_End())
            {
                return true;
            }
            else if (Write())
            {
                return true;
            }
            else if (Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea operatiei de asignare
        /// </summary>
        /// <returns>false in cazul unei erori sau a cazului in care nu este vorba de o asignare</returns>
        private bool Asignare()
        {
            if ((int)curent_atom.indice == (int)define.IDENTIFIER)
            {

                if (lista_variabile.Contains(curent_atom.valoare) == true)
                {
                    pr = "secund";
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    tip_variabila = (string)lista_variabile[curent_atom.valoare];
                    NextAtom();
                    if ((int)curent_atom.indice == (int)define.ASSIGN)
                    {
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                        Expresie_repetare();
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare: Lipseste operatorul de atribuire:, \":=\"!");
                        AtomSincronizare();
                        return true;
                    }
                    return true;
                }
                else if ((int)urmator_atom.indice == (int)define.PARDL)
                    {
                        Atom curent = new Atom(curent_atom.indice, curent_atom.valoare, curent_atom.linie);
                        NextAtom();
                        if ((int)curent_atom.indice == (int)define.PARDL)
                        {
                            NextAtom();
                            if ((int)curent_atom.indice == (int)define.NRINTEGER)
                            {
                                int valoare = System.Convert.ToInt32(curent_atom.valoare);
                                NextAtom();
                                if ((int)curent_atom.indice == (int)define.PARDR)
                                {
                                    NextAtom();
                                    AdaugareInCod(TranslatareAtom(curent) + "[" + valoare.ToString() + "]");
                                    if ((int)curent_atom.indice == (int)define.ASSIGN)
                                    {
                                        AdaugareInCod(TranslatareAtom(curent_atom));
                                        NextAtom();
                                        Expresie_repetare();
                                    }
                                    else
                                    {
                                        eroare = true;
                                        Errors("Eroare: Lipseste operatorul de atribuire:, \":=\"!");
                                        AtomSincronizare();
                                        return true;
                                    }
                                    return true;
                                }
                                else
                                {
                                    eroare = true;
                                    Errors("Eroare: Nu ati introdus operatorul \"]\"!");
                                    AtomSincronizare();
                                    return true;
                                }
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Identificatorul nu este un numar de tip intreg!");
                                AtomSincronizare();
                                return true;
                            }
                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Variabila inexistenta!");
                            AtomSincronizare();
                            return true;
                        }
                    }
                else
                {
                    eroare = true;
                    Errors("Eroare: Variabila nedefinita: " + curent_atom.valoare);
                    return true;
                }
            }
            else
            {
                return false;
            }
            
        }
        /// <summary>
        /// tratarea expresiilor
        /// </summary>
        /// <returns></returns>
        private bool Expresie_repetare()
        {
            Termen();
            Expresie();
            return true;
        }
        /// <summary>
        /// identificarea expresiei curente
        /// </summary>
        /// <returns></returns>
        private bool Expresie()
        {
            if (Operatie_si_minus() == true)
            {
                Termen();
                Expresie();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea operatiilor matematice de gradul intai, adica adunare si scade
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Operatie_si_minus()

        {
            if ((int)curent_atom.indice == (int)define.PLUS)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.MINUS)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea operatiilor matematice de gradul al doilea, adica inmultire si impartire
        /// </summary>
        /// <returns></returns>
        private bool Operatie_Inm_Imp()
        {
            if ((int)curent_atom.indice == (int)define.MUL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.DIV)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.MOD)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// identificarea termenilor care fac parte din expresie
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Termen()
        {
            Operatii();
            Termeni();
            return true;
        }
        /// <summary>
        /// identificarea termenilor care fac parte din expresie
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Termeni()
        {
            if (Operatie_Inm_Imp() == true)
            {
                Operatii();
                Termeni();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// identificarea termenilor care fac parte din expresie
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Operatii()
        {
            if ((int)curent_atom.indice == (int)define.IDENTIFIER)
            {

                if (lista_variabile.Contains(curent_atom.valoare) == true)
                {
                    if (lista_variabile.Contains(curent_atom.valoare) == true)
                    {
                        if (pr.CompareTo("prim") != 0)
                        {
                            string tip_va = "";
                            if (((string)(lista_variabile[curent_atom.valoare])).CompareTo("integer") == 0)
                            {
                                tip_va = "integer";
                            }
                            else if (((string)(lista_variabile[curent_atom.valoare])).CompareTo("real") == 0)
                            {
                                tip_va = "real";
                            }
                            else if (((string)(lista_variabile[curent_atom.valoare])).CompareTo("long int") == 0)
                            {
                                tip_va = "long int";
                            }
                            else if (((string)(lista_variabile[curent_atom.valoare])).CompareTo("char") == 0)
                            {
                                tip_va = "char";
                            }
                            if (tip_variabila.CompareTo(tip_va) == 0)
                            {
                                AdaugareInCod(TranslatareAtom(curent_atom));
                                NextAtom();
                                return true;
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Variabila de tip: " + tip_variabila + ", nu se poate converta in: " + tip_va);
                                AtomSincronizare();
                                return true;
                            }
                        }
                        else
                        {
                            AdaugareInCod(TranslatareAtom(curent_atom));
                            tip_variabila = (string)lista_variabile[curent_atom.valoare];
                            NextAtom();
                            return true;
                        }
                    }
                }
                else
                {
                    Atom curent = new Atom(curent_atom.indice, curent_atom.valoare, curent_atom.linie);
                    NextAtom();
                    if ((int)curent_atom.indice == (int)define.PARDL)
                    {
                        NextAtom();
                        if ((int)curent_atom.indice == (int)define.NRINTEGER)
                        {
                            int valoare = System.Convert.ToInt32(curent_atom.valoare);
                            NextAtom();
                            if ((int)curent_atom.indice == (int)define.PARDR)
                            {
                                NextAtom();
                                string temp2 = TranslatareAtom(curent) + "[" + valoare.ToString() + "]";
                                if (lista_variabile.Contains(temp2) == true)
                                {
                                    AdaugareInCod(temp2);
                                    return true;
                                }
                                else
                                {
                                    eroare = true;
                                    Errors("Eroare: Variabila \"" + temp2 + "\" nu a fost declarata!");
                                    AtomSincronizare();
                                    return true;
                                }
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Nu ati introdus operatorul \"]\"!");
                                AtomSincronizare();
                                return true;
                            }
                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Identificatorul nu este un numar de tip intreg!");
                            AtomSincronizare();
                            return true;
                        }
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare: Variabila sau constanta nu exista!");
                        AtomSincronizare();
                        return true;
                    }
                }
            }
            else if (Constante() == true)
            {
                return true;
            }
            else if ((int)curent_atom.indice == (int)define.PARL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                if (Expresie_repetare() == true)
                {
                    if ((int)curent_atom.indice == (int)define.PARR)
                    {
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare: Nu ati introdus paranteza \")\"!");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea cazului in care avem constante
        /// </summary>
        /// <returns>false in cazul in care nu avem constante sau avem o eroare</returns>
        private bool Constante()
        {
            if ((int)curent_atom.indice == (int)define.NRINTEGER)
            {
                if (tip_variabila.CompareTo("integer") == 0)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Variabila la care se asigneaza este de tip: " + tip_variabila + " nu de tip integer!");
                }
            }
            else if ((int)curent_atom.indice == (int)define.NRREAL)
            {
                if (tip_variabila.CompareTo("real") == 0)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Variabila la care se asigneaza es de tip: " + tip_variabila + " nu de tip real!");
                }
            }
            else if ((int)curent_atom.indice == (int)define.SIRCARACTERE)
            {
                if (tip_variabila.CompareTo("char") == 0)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Variabila la care se asigneaza es de tip: " + tip_variabila + " nu de tip char!");
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea instructiunii if
        /// </summary>
        /// <returns>false in cazul in care nu este instructiune if sau exista o eroare</returns>
        private bool If()
        {
            if ((int)curent_atom.indice == (int)define.IF)
            {
                pr = "prim";
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                Operatii_logica_negare();
                Expresii_logice_repetare();
                If_log_cicle();
                if ((int)curent_atom.indice == (int)define.THEN)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom) + "\r\n");
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Lipseste cuvantul cheie \"then\"!");
                    AtomSincronizare();
                    return true;
                }
                Instructii();
                If_Else();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea operatiei logice de negare
        /// </summary>
        /// <returns></returns>
        private bool Operatii_logica_negare()
        {
            if ((int)curent_atom.indice == (int)define.NOT)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            return Operatii_logice();
        }
        /// <summary>
        /// tratarea operatiilo logice
        /// </summary>
        /// <returns>false in cazul in care nu avem o operatire logica sau avem o eroare</returns>
        private bool Operatii_logice()
        {
            if ((int)curent_atom.indice == (int)define.AND)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.OR)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// functie de tratare a operatiilor logice
        /// </summary>
        /// <returns>false in cazul in care avem o eroare</returns>
        private bool Expresii_logice_repetare()
        {
            Expresii_Relationale();
            Expresii_logice();
            return true;
        }
        /// <summary>
        /// identificarea operatiilor logice
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Expresii_logice()
        {
            if (Operatii_logice() == true)
            {
                Expresii_Relationale();
                Expresii_logice();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Identificarea expresiilor relationale
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool Expresii_Relationale()
        {
            AdaugareInCod("(");
            Expresie_repetare();
            if (Operatii_relationale() == false)
            {
                eroare = true;
                Errors("Eroare: Lipseste operatia relationala!");
                AtomSincronizare();
                return true;
            }
            Expresie_repetare();
            AdaugareInCod(")");
            return true;
        }
        /// <summary>
        /// identificarea operatiilor relatiionale
        /// </summary>
        /// <returns>false in cazul unei erori sau in cazul in care nu avem o operatie relationala</returns>
        private bool Operatii_relationale()
        {
            if ((int)curent_atom.indice == (int)define.MAIMARE)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.MAIMIC)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.MAIMARESAUEGAL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.MAIMICSAUEGAL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.NOTEQUAL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.EQUAL)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// traterea cazului in caze avem un bloc if cu m ai multe definitii
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool If_log_cicle()
        {
            if (If_lob_next() == true)
            {
                If_log_cicle();
                return true;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// tratarea urmatorului caz din cadrul operatiei if
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool If_lob_next()
        {
            if (Operatii_logica_negare() == true)
            {
                Expresii_logice_repetare();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea cazului in care avem un bloc if then else
        /// </summary>
        /// <returns>false in cazul unei erori</returns>
        private bool If_Else()
        {
            if ((int)curent_atom.indice == (int)define.ELSE)
            {
                AdaugareInCod(TranslatareAtom(curent_atom) + "\r\n");
                NextAtom();
                Instructii();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea blocului while
        /// </summary>
        /// <returns>false in cazul unei erori sau nu avem un bloc de tip while</returns>
        private bool While()
        {
            if ((int)curent_atom.indice == (int)define.WHILE)
            {
                pr = "prim";
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                Operatii_logica_negare();
                Expresii_logice_repetare();
                If_log_cicle();
                if ((int)curent_atom.indice == (int)define.DO)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus cuvantul cheie \"do\"!");
                    AtomSincronizare();
                    return true;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// tratarea blocului for
        /// </summary>
        /// <returns>false in cazul unei erori sau nu avem un bloc for</returns>
        private bool For()
        {
            if ((int)curent_atom.indice == (int)define.FOR)
            {
                pr = "prim";
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                if ((int)curent_atom.indice == (int)define.IDENTIFIER)
                {
                    if (lista_variabile.Contains(curent_atom.valoare) == true)
                    {
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                    }
                    else
                    {
                        eroare = true;
                        Errors("Variabila: \"" + curent_atom.valoare + "\", nu exista");
                        AtomSincronizare();
                        return true;
                    }
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Liseste variabila de control!");
                    AtomSincronizare();
                    return true;
                }
                if ((int)curent_atom.indice == (int)define.ASSIGN)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Lipseste operatorul de asignare \":=\"!");
                    AtomSincronizare();
                    return true;
                }
                Expresie_repetare();
                cale();
                Expresie_repetare();
                Step();
                if ((int)curent_atom.indice == (int)define.DO)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus cuvantul cheie \"do\"!");
                    AtomSincronizare();
                    return true;
                }
                Instructii();
                return true;
            }
            return false;
        }
        /// <summary>
        /// determinarea corectitudinii blocului for
        /// </summary>
        /// <returns></returns>
        private bool cale()
        {
            if ((int)curent_atom.indice == (int)define.TO)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else if ((int)curent_atom.indice == (int)define.DOWNTO)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
            }
            else
            {
                eroare = true;
                Errors("Eroare: Nu ati introdus cuvantul cheie \"to\" sau \"downto\"!");
                AtomSincronizare();
                return true;
            }
            return true;
        }
        /// <summary>
        /// determinarea corectitudinii blocului for
        /// </summary>
        /// <returns></returns>
        private bool Step()
        {
            if ((int)curent_atom.indice == (int)define.STEP)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                Expresie_repetare();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea blocului repeat
        /// </summary>
        /// <returns></returns>
        private bool Repeat()
        {
            if ((int)curent_atom.indice == (int)define.REPEAT)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                Instructii();
                if ((int)curent_atom.indice == (int)define.UNTIL)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus cuvantul cheie \"until\"!");
                    AtomSincronizare();
                    return true;
                }
                pr = "prim";
                Operatii_logica_negare();
                Expresii_logice_repetare();
                If_log_cicle();
                return true;
            }
            return false;
        }
        /// <summary>
        /// tratarea procedurii print
        /// </summary>
        /// <returns></returns>
        private bool Write()
        {
            if ((int)curent_atom.indice == (int)define.WRITE)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                if ((int)curent_atom.indice == (int)define.PARL)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                    if ((int)curent_atom.indice == (int)define.GHILIMELESIMPLE || (int)curent_atom.indice == (int)define.GHILIMELE)
                    {
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                        while ((int)curent_atom.indice != (int)define.GHILIMELESIMPLE && (int)curent_atom.indice != (int)define.GHILIMELE)
                        {
                            if ((int)curent_atom.indice != (int)define.SIRCARACTERE)
                            {
                                eroare = true;
                                Errors("Eroare: Nu ati introdus \" sau \"!");
                                return false;
                            }
                            else
                            {
                                AdaugareInCod(TranslatareAtom(curent_atom) + " ");
                                NextAtom();
                            }
                        }
                        AdaugareInCod(TranslatareAtom(curent_atom));
                        NextAtom();
                        if ((int)curent_atom.indice == (int)define.PARR)
                        {
                            AdaugareInCod(TranslatareAtom(curent_atom));
                            NextAtom();
                            return true;
                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Nu ati introdus \")\" sau \"!");
                            return false;

                        }
                    }
                    else if (curent_atom.indice == (int)define.IDENTIFIER)
                    {
                        if (lista_variabile.Contains(curent_atom.valoare))
                        {
                            AdaugareInCod(TranslatareAtom(curent_atom));
                            NextAtom();
                            if ((int)curent_atom.indice == (int)define.PARR)
                            {
                                AdaugareInCod(TranslatareAtom(curent_atom));
                                NextAtom();
                                return true;
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Nu ati introdus \")\" sau \"!");
                                return false;

                            }
                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Variabila \"" + curent_atom.valoare + "\", nu exista!");
                            return false;
                        }
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare de sintaxa!");
                        return false;
                    }
                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus \"(\" sau \"!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// tratarea procedurii read
        /// </summary>
        /// <returns></returns>
        private bool Read()
        {
            if ((int)curent_atom.indice == (int)define.READ)
            {
                AdaugareInCod(TranslatareAtom(curent_atom));
                NextAtom();
                if ((int)curent_atom.indice == (int)define.PARL)
                {
                    AdaugareInCod(TranslatareAtom(curent_atom));
                    NextAtom();
                    if ((int)curent_atom.indice == (int)define.IDENTIFIER)
                    {
                        if (lista_variabile.Contains(curent_atom.valoare) == true)
                        {
                            AdaugareInCod(TranslatareAtom(curent_atom));
                            NextAtom();
                            if ((int)curent_atom.indice == (int)define.PARR)
                            {
                                AdaugareInCod(TranslatareAtom(curent_atom));
                                NextAtom();
                                return true;
                            }
                            else
                            {
                                eroare = true;
                                Errors("Eroare: Nu ati introdus \")\"!");
                                return false;
                            }

                        }
                        else
                        {
                            eroare = true;
                            Errors("Eroare: Variabila \"" + curent_atom.valoare + "\", nu exista!");
                            return false;
                        }
                    }
                    else
                    {
                        eroare = true;
                        Errors("Eroare de sintaxa");
                        return false;
                    }

                }
                else
                {
                    eroare = true;
                    Errors("Eroare: Nu ati introdus \"(\"!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
