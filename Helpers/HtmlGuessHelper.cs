using RandomNumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RandomNumber.Helpers
{
    public static class HtmlGuessHelper
    {
        public static MvcHtmlString Home(IList<NumberGenerator> ListDerived)
        {

            TagBuilder container = new TagBuilder("div");
            container.MergeAttribute("class", "col-md-12 center");

            TagBuilder divInline = new TagBuilder("div");
            divInline.MergeAttribute("style", "display:inline");

            TagBuilder title = new TagBuilder("h1");
            title.InnerHtml += "Random Number Guessing Game";

            TagBuilder br = new TagBuilder("br");

            divInline.InnerHtml += title;
            divInline.InnerHtml += br;

            TagBuilder btnGroup = new TagBuilder("div");
            btnGroup.MergeAttribute("class", "btn-group");

            foreach (NumberGenerator derived in ListDerived)
            {
                TagBuilder btn = new TagBuilder("button");
                btn.MergeAttribute("class", "btn btn-default btn-lg");
                btn.MergeAttribute("onclick", $"newGame({derived.Level})");
                btn.InnerHtml = derived.LevelDescription;
                btnGroup.InnerHtml += btn;
            }
            divInline.InnerHtml += btnGroup;
            container.InnerHtml += divInline;

            TagBuilder gameTag = new TagBuilder("div");
            gameTag.MergeAttribute("id", "game");

            container.InnerHtml += gameTag;

            return new MvcHtmlString(container.ToString());
        }
        public static MvcHtmlString Form(NumberGenerator Generator)
        {
            TagBuilder container = new TagBuilder("div");

            TagBuilder inputLevel = new TagBuilder("input");
            inputLevel.MergeAttribute("id", "level");
            inputLevel.MergeAttribute("type", "hidden");
            inputLevel.MergeAttribute("value", Generator.Level.ToString());

            TagBuilder inputRandom = new TagBuilder("input");
            inputRandom.MergeAttribute("id", "random");
            inputRandom.MergeAttribute("type", "hidden");
            inputRandom.MergeAttribute("value", Generator.Random.ToString());

            TagBuilder range = new TagBuilder("h2");
            range.MergeAttribute("id", "range");
            range.MergeAttribute("data-range", Generator.Range.ToString());
            range.InnerHtml += "Guess the number between 1 and " + Generator.Range.ToString();

            container.InnerHtml += inputLevel;
            container.InnerHtml += inputRandom;
            container.InnerHtml += range;

            container.InnerHtml += DivGuess(Generator.Random, Generator.Range,
                Generator.Guesses, Generator.Guess);

            return new MvcHtmlString(container.ToString());
        }
        public static MvcHtmlString Guess(NumberGenerator Random, List<string> errors)
        {
            TagBuilder container = new TagBuilder("div");
            container.InnerHtml += DivGuess(Random.Random, Random.Range, Random.Guesses, Random.Guess);
            if (errors != null)
            {
                foreach (string error in errors)
                {
                    TagBuilder err = new TagBuilder("p");
                    err.MergeAttribute("class", "fail");
                    err.InnerHtml = error;
                    container.InnerHtml += err;
                }
                return new MvcHtmlString(container.ToString());
            }

            container.InnerHtml += PlayMessage(Random.Random, Random.Guess);

            if (Random.Guesses == 0)
            {
                container.InnerHtml += GameOver(Random.Random, Random.Guess);
            }

            return new MvcHtmlString(container.ToString());
        }
        private static MvcHtmlString Textbox(int Random, int Range, int Guesses, int Guess)
        {
            TagBuilder container = new TagBuilder("h2");

            TagBuilder inputGuess = new TagBuilder("input");
            inputGuess.MergeAttribute("id", "guess");
            inputGuess.MergeAttribute("type", "number");
            inputGuess.MergeAttribute("step", "1");
            inputGuess.MergeAttribute("min", "1");
            inputGuess.MergeAttribute("max", Range.ToString());
            inputGuess.MergeAttribute("MaxLength ", Range.ToString().Length.ToString());
            inputGuess.MergeAttribute("onKeyDown", "if(event.keyCode==13) validate_guess();");
            inputGuess.MergeAttribute("oninput", "this.value=this.value.slice(0,this.maxLength)");

            //oninput="this.value=this.value.slice(0,this.maxLength)"
            if (Guesses == 0 || Random == Guess)
            {
                inputGuess.MergeAttribute("disabled", "disabled");
                if (Random == Guess)
                {
                    inputGuess.MergeAttribute("value", Random.ToString());
                }
            }

            container.InnerHtml += inputGuess;
            return new MvcHtmlString(container.ToString());
        }
        private static MvcHtmlString SubmitButton(int Random, int Guesses, int Guess)
        {
            TagBuilder btnGuess = new TagBuilder("button");
            btnGuess.MergeAttribute("class", "btn btn-default btn-lg");
            btnGuess.MergeAttribute("onclick", "validate_guess()");
            btnGuess.InnerHtml += "Guess";

            if (Guesses == 0 || Random == Guess)
            {
                btnGuess.MergeAttribute("disabled", "disabled");
            }
            return new MvcHtmlString(btnGuess.ToString());
        }
        private static MvcHtmlString DivGuess(int Random, int Range, int Guesses, int Guess)
        {
            TagBuilder divGuess = new TagBuilder("div");
            divGuess.MergeAttribute("id", "div_guess");

            TagBuilder bGuesses = new TagBuilder("b");
            bGuesses.MergeAttribute("id", "guesses");
            bGuesses.InnerHtml += Guesses.ToString();

            TagBuilder pGuesses = new TagBuilder("h2");
            pGuesses.InnerHtml += "you have ";
            pGuesses.InnerHtml += bGuesses;
            pGuesses.InnerHtml += " guesses";

            divGuess.InnerHtml += pGuesses;

            divGuess.InnerHtml += Textbox(Random, Range, Guesses, Guess);
            divGuess.InnerHtml += SubmitButton(Random, Guesses, Guess);

            return new MvcHtmlString(divGuess.ToString());
        }
        private static MvcHtmlString GameOver(int Random, int Guess)
        {
            TagBuilder container = new TagBuilder("h2");
            if (Random != Guess)
            {
                container.MergeAttribute("class", "fail");
                container.InnerHtml += "You lost! Play again?";
            }
            else
            {
                container.MergeAttribute("class", "success");
                container.InnerHtml += "You won! Play again?";
            }
            return new MvcHtmlString(container.ToString());
        }
        private static MvcHtmlString PlayMessage(int Random, int Guess)
        {
            string msg = "";
            TagBuilder container = new TagBuilder("h2");

            switch (Guess)
            {
                case int n when n == Random:
                    msg = "Success!! You guessed the correct number <b>"
                        + Random.ToString() + "<b>";
                    container.MergeAttribute("class", "success");

                    break;
                case int n when n > Random:
                    msg = "Fail!! You guessed too high!";
                    container.MergeAttribute("class", "fail");
                    break;
                case int n when n < Random:
                    msg = "Fail!! You guessed too low!";
                    container.MergeAttribute("class", "fail");
                    break;
            }
            container.InnerHtml += msg;
            return new MvcHtmlString(container.ToString());
        }
    }
}