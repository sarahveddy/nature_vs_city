from datetime import datetime


def create(db):
    class BejeweledStats(db.Model):
        __tablename__ = "bejeweled_stats"

        bejeweledStatID = db.Column(db.Integer, primary_key=True, autoincrement=True)
        participantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
        submitTime = db.Column(db.DateTime, nullable=False, default=datetime.min)
        level = db.Column(db.Integer, nullable=False, default=-1) # 0 for forest and 1 for urban
        quitTime = db.Column(db.String, nullable=False, default=-1) # in seconds
        quitButtonPressed = db.Column(db.Integer, nullable=False, default=False)


    return BejeweledStats



    # colby's code

    # class SHTrial(db.Model):
    #     __tablename__ = "sh_trial"
    #
    #     shStateID = db.Column(db.Integer, primary_key=True, autoincrement=True)
    #     participantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
    #     submitTime = db.Column(db.DateTime, nullable=False, default=datetime.min)
    #     duration = db.Column(db.Float, nullable=False, default=0.0)
    #     avgFps = db.Column(db.Float, nullable=False, default=0.0)
    #     trialNumber = db.Column(db.Integer, nullable=False, default=0)
    #     sessionNumber = db.Column(db.Integer, nullable=False, default=0)
    #     difficultyRotation = db.Column(db.Float, nullable=False, default=0.0)
    #     difficultySpawning = db.Column(db.Float, nullable=False, default=0.0)