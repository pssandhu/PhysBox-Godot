using Godot;
using System;

public class LineEditValidation : LineEdit {

    private String oldText;

    [Export(PropertyHint.Enum, "Text,Integer,Decimal")]
    private string inputFormat = "Text";

    public override void _Ready() {
        this.Connect("text_changed" , this, "ValidateInput");
        oldText = Text;
    }

    private void ValidateInput(string newText) {
        if (inputFormat != "Text" && newText != "" && newText != "-" && newText != "+") {
            int newCaretPosition = CaretPosition;
            bool invalidInput = false;

            if (inputFormat == "Integer") {
                invalidInput = !newText.IsValidInteger();
            } else if (inputFormat == "Decimal") {
                invalidInput = !newText.IsValidFloat();
            }

            if (invalidInput) {
                GD.Print("Not a valid " + inputFormat);
                Text = oldText;

                if (newText.Length == 1) {
                    // If newText is a single invalid character, move the caret
                    // to the end of the oldText e.g. if the user selected all
                    // the text then entered an invalid character
                    CaretPosition = oldText.Length;
                } else {
                    CaretPosition = newCaretPosition - 1;
                }
            } else {
                // Remove leading and trailing spaces
                String trimmedText = newText.TrimStart();
                bool startTrimmed = newText != trimmedText;
                trimmedText = trimmedText.TrimEnd();

                Text = trimmedText;
                oldText = trimmedText;

                // Caret gets moved to the start so move it to the correct place
                if (startTrimmed) {
                    CaretPosition = 0;
                } else {
                    CaretPosition = newCaretPosition;
                }
            }
        } else {
            oldText = newText;
        }
    }

}
