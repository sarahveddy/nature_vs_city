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
        trial = db.BejeweledStats()
        try:
            trial.participantID = session['participantID']
        except: # This because I sometimes test from Unity directly
            trial.participantID = request.form['participantID']
        trial.submitTime = datetime.now()
        trial.level = request.form['level']

        db.session.add(trial)
        db.session.commit()

        return ""

    return render_template("bejeweled.html",
                           crumbs=create_breadcrumbs(), application_root=current_app.config["APPLICATION_ROOT"])



# @superhexagon.route("/sh_post_trial", methods=['POST'])
# def sh_post_trial():
#     if request.method == 'POST':
#
#         trial = db.SHTrial()
#         try:
#             trial.participantID = session['participantID']
#         except: # This because I sometimes test from Unity directly
#             trial.participantID = request.form['participantID']
#         trial.submitTime = datetime.now()
#         trial.duration = float(request.form["duration"])
#         trial.avgFps = float(request.form["avgFps"])
#         trial.trialNumber = int(request.form["trialNumber"])
#         trial.sessionNumber = int(request.form["sessionNumber"])
#         trial.difficultyRotation = float(request.form["difficultyRotation"])
#         trial.difficultySpawning = float(request.form["difficultySpawning"])
#         trial.movements = request.form["movements"]
#
#         db.session.add(trial)
#         db.session.commit()
#
#         return ""


