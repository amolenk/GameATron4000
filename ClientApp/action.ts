/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { RoomObject } from "./room-object"

export class Action {

    public static GiveVerb: string = "Give";
    public static PickUpVerb: string = "Pick up";
    public static UseVerb: string = "Use";
    public static OpenVerb: string = "Open";
    public static LookAtVerb: string = "Look at";
    public static PushVerb: string = "Push";
    public static CloseVerb: string = "Close";
    public static TalkToVerb: string = "Talk to";
    public static PullVerb: string = "Pull";

    private subjects: Array<RoomObject>;

    constructor(public text: string, private subjectSeparator?: string) {
        this.subjects = new Array<RoomObject>();
    }

    public addSubject(subject: RoomObject) {

        var isComplexAction = this.subjectSeparator != null;

        if (isComplexAction) {

            // For complex actions (Give, Use), the first subject must always
            // be an inventory item.
            if (this.subjects.length == 0 && !subject.name.startsWith("inventory-")) {
                return false;
            }

            // For the Give action, the second subject must always be an actor.
            if (this.text == Action.GiveVerb
                && this.subjects.length == 1
                && !subject.name.startsWith("actor-")) {
                return false;
            }

            // Don't add the subject if it's the same as an already selected subject.
            if (this.subjects.length == 1 && this.subjects[0] == subject) {
                return false;
            }
        }

        this.subjects.push(subject);

        var actionComplete = this.subjects.length == (isComplexAction ? 2 : 1);

        return actionComplete;
    }

    public getDisplayText(roomObject?: RoomObject) {

        // Ignore the current room object that the mouse is over if it's the same as
        // the first subject (if any).
        if (this.subjects.length == 1 && this.subjects[0] == roomObject) {
            roomObject = null;
        }

        if (this.subjects.length == 2) {
            return `${this.text} ${this.subjects[0].displayName} ${this.subjectSeparator} ${this.subjects[1].displayName}`;
        }

        if (this.subjects.length == 1) {

            if (roomObject != null) {
                return `${this.text} ${this.subjects[0].displayName} ${this.subjectSeparator} ${roomObject.displayName}`;
            }

            if (this.subjectSeparator != null) {
                return `${this.text} ${this.subjects[0].displayName} ${this.subjectSeparator}`;
            }

            return `${this.text} ${this.subjects[0].displayName}`;
        }

        if (this.subjects.length == 0 && roomObject != null) {
            return `${this.text} ${roomObject.displayName}`;
        }

        return this.text;
    }
}
