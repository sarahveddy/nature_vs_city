from datetime import datetime

from flask import Blueprint, render_template

from BOFS.util import *
from BOFS.globals import db

bejeweled = Blueprint('bejeweled', __name__,
                      static_url_path='/bejeweled', template_folder='templates', static_folder='static')


@bejeweled.route("/intro")
@verify_correct_page
@verify_session_valid
def intro():
    return render_template("intro.html")


@bejeweled.route("/game_bejeweled", methods=['POST', 'GET'])
@verify_correct_page
@verify_session_valid
def game_bejeweled():
    if request.method == 'POST':
        if not 'action' in request.form or request.form['action'] != "gameStats":
            return ""

        stat = db.BejeweledStats()
        stat.timeCompleted = datetime.now()
        stat.participantID = session['participantID']

        db.session.add(stat)
        db.session.commit()

        return ""

    return render_template("bejeweled.html",
                           crumbs=create_breadcrumbs(), application_root=current_app.config["APPLICATION_ROOT"])
