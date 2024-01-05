using System;
using System.Collections.Generic;
using System.Data;
using practiceGameChallenges.ObjectRepository;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities.TestData;
using UiPath.Testing.Activities.TestDataQueues.Enums;
using UiPath.Testing.Enums;
using UiPath.UIAutomationNext.API.Contracts;
using UiPath.UIAutomationNext.API.Models;
using System.Linq;

namespace practiceGameChallenges
{
    public class TicTacToeGame
    {
        public string botSymbol;
        public string opponentSymbol;
        public bool playPoorly;
        public String[,] matrix;
        public TicTacToeGame(string nBotSymbol, bool nPlayPoorly)
        {
            botSymbol=nBotSymbol;
            opponentSymbol = botSymbol=="x" ? "o" : "x";
            playPoorly=nPlayPoorly;
            matrix = new String[3, 3];
        }
        
        public (int, int) chooseNextMove(){
            (int,int) selected_move = bestMove();
            matrix[selected_move.Item1,selected_move.Item2]=botSymbol;
            return selected_move;
        }
        
        public int minimax(bool opponentPlayer){
            //const winner = this.any_winner(position);
            var minimizeScore = (opponentPlayer !=  playPoorly);
            var winner = anySolution();
            List<(int,int)> availableMoves_ = availableMoves();
            int depth = availableMoves_.Count;
            // if there is a winner and (or is computer player or play poorly is true)
            if(winner && opponentPlayer) return 100+depth;
            // otherwise, if winner 
            if(winner) return -100-depth;
            if(depth==0) return 0;
            var listScores = new List<int>();
            var best_score = minimizeScore ? 200 : -200;
            foreach((int,int) move in availableMoves_){
                matrix[move.Item1,move.Item2]= opponentPlayer ? opponentSymbol : botSymbol;
                var new_score = minimax(!opponentPlayer);
                matrix[move.Item1,move.Item2]= "";
                listScores.Add(new_score);
            };
            return minimizeScore ? listScores.Min() : listScores.Max();
        }
        
        public string curretPosition(){
            var sol=new List<string>();
            for(int i=0; i<3;i++){
             for(int j=0; j<3;j++){
                sol.Add(matrix[i,j]);
            }   
            }
            return string.Join(",",sol);
        }
        
        
        
        public (int,int) bestMove(){
            List<(int,int)> availableMoves_ = availableMoves();
            int depth = availableMoves_.Count;
            int best_score = playPoorly ? 200 : -200;
            List<(int,int)> best_moves = new List<(int,int)>();
            foreach((int,int) move in availableMoves_){
                matrix[move.Item1,move.Item2]=botSymbol;
                var new_score = minimax(true);
                matrix[move.Item1,move.Item2]="";
                if((!playPoorly && new_score>best_score) || (playPoorly && new_score<best_score)){
                    best_score=new_score;
                    best_moves = new List<(int,int)>();
                    best_moves.Add(move);
                }
                else if(new_score==best_score) best_moves.Add(move);
            }
            Random rnd = new Random();
            int randomIndex = rnd.Next(best_moves.Count); 
            return best_moves[randomIndex];
        }
        
        public void getRowCol(out int row, out int col, int index)
        {
            row= index%3;
            col=(int)Math.Floor(index/3.0);
        }

        
        public string calculateNextPlayer(){
            int nonEmpty=0;
            int row=0;
            int col=0;
        for(int i=0;i<9;i++){
            getRowCol(out row,out col, i);
            if(matrix[row,col]!="")nonEmpty++;
        }
        if(nonEmpty%2==0) return "x";
        return "o";
        }
        
        public List<(int,int)> availableMoves(){
            List<(int,int)> moves = new List<(int,int)>();
            int row=0;
            int col=0;
            for(int i=0;i<9;i++){
            getRowCol(out row,out col, i);
            if(matrix[row,col]=="") moves.Add((row,col));
            }
            return moves;
        }
        
        public void GetNewMatrix(List<String> nCells){
            for(int i=0; i<9;i++){
                int row= (int)Math.Floor(i/3.0);
                int col= i % 3;
                matrix[row,col]="";
                if(nCells[i].Contains("src=\"./static/media/x")){
                    matrix[row,col]="x";
                }
                else if(nCells[i].ToString().Contains("src=\"./static/media/o")){
                    matrix[row,col]="o";
                }
                else{
                    matrix[row,col]="";
                }
                Console.WriteLine(matrix[row,col]);
            }
            Console.WriteLine("get new matrix execution completed");
        }
        
        public void AssignNewMatrix(List<String> nCells){
            for(int i=0; i<9;i++){
                int row= (int)Math.Floor(i/3.0);
                int col= i % 3;
                matrix[row,col]=nCells[i];
            }
            Console.WriteLine("assignation was completed");
        }
        
        
        public bool anySolution(){
            List<string> solutions = new List<string>{"xxx","ooo"};
            List<string> rowSols = new List<string> { matrix[0,0]+ matrix[0,1]+matrix[0,2], matrix[1,0]+ matrix[1,1]+matrix[1,2], matrix[2,0]+ matrix[2,1]+matrix[2,2] };
            if(rowSols.Exists(s=> solutions.Contains(s))) return true;
            List<string> columnSols = new List<string> { matrix[0,0]+ matrix[1,0]+matrix[2,0], matrix[0,1]+ matrix[1,1]+matrix[2,1],matrix[0,2]+ matrix[1,2]+matrix[2,2] };
            if(columnSols.Exists(s=> solutions.Contains(s))) return true;
            List<string> diagonalSols = new List<string> { matrix[0,0]+ matrix[1,1]+matrix[2,2], matrix[0,2]+ matrix[1,1]+matrix[2,0] };
            if(diagonalSols.Exists(s=> solutions.Contains(s))) return true;
            return false;
        }
    }
}