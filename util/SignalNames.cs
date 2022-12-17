/// <summary>
/// Contains const strings of common signals, to make the API less stringy.
/// </summary>
public static class SignalNames
{
    public const string NODE_CHILD_ENTERED_TREE = "child_entered_tree";
    public const string NODE_CHILD_EXITING_TREE = "child_exiting_tree";
    public const string NODE_READY = "ready";
    public const string NODE_RENAMED = "renamed";
    public const string NODE_TREE_ENTERED = "tree_entered";
    public const string NODE_TREE_EXITED = "tree_exited";
    public const string NODE_TREE_EXITING = "tree_exiting";

    public const string CANVASITEM_DRAW = "draw";
    public const string CANVASITEM_HIDE = "hide";
    public const string CANVASITEM_RECT_CHANGED = "item_rect_changed";
    public const string CANVASITEM_VISIBILITY_CHANGED = "visibility_changed";

    public const string SPRITE_FRAME_CHANGED = "frame_changed";
    public const string SPRITE_TEXTURE_CHANGED = "texture_changed";

    public const string COLLISIONOBJECT2D_INPUT_EVENT = "input_event";
    public const string COLLISIONOBJECT2D_MOUSE_ENTERED = "mouse_entered";
    public const string COLLISIONOBJECT2D_MOUSE_EXITED = "mouse_exited";
    
    public const string COLLISIONOBJECT3D_INPUT_EVENT = "input_event";
    public const string COLLISIONOBJECT3D_MOUSE_ENTERED = "mouse_entered";
    public const string COLLISIONOBJECT3D_MOUSE_EXITED = "mouse_exited";

    public const string AREA2D_AREA_ENTERED = "area_entered";
    public const string AREA2D_AREA_EXITED = "area_exited";
    public const string AREA2D_AREA_SHAPE_ENTERED = "area_shape_entered";
    public const string AREA2D_AREA_SHAPE_EXITED = "area_shape_exited";
    public const string AREA2D_BODY_ENTERED = "body_entered";
    public const string AREA2D_BODY_EXITED = "body_exited";
    public const string AREA2D_BODY_SHAPE_ENTERED = "body_shape_entered";
    public const string AREA2D_BODY_SHAPE_EXITED = "body_shape_exited";

    public const string RIGIDBODY2D_BODY_ENTERED = "body_entered";
    public const string RIGIDBODY2D_BODY_EXITED = "body_exited";
    public const string RIGIDBODY2D_BODY_SHAPE_ENTERED = "body_shape_entered";
    public const string RIGIDBODY2D_BODY_SHAPE_EXITED = "body_shape_exited";
    public const string RIGIDBODY2D_SLEEPING_STATE_CHANGED = "sleeping_state_changed";
    
    public const string AREA3D_AREA_ENTERED = "area_entered";
    public const string AREA3D_AREA_EXITED = "area_exited";
    public const string AREA3D_AREA_SHAPE_ENTERED = "area_shape_entered";
    public const string AREA3D_AREA_SHAPE_EXITED = "area_shape_exited";
    public const string AREA3D_BODY_ENTERED = "body_entered";
    public const string AREA3D_BODY_EXITED = "body_exited";
    public const string AREA3D_BODY_SHAPE_ENTERED = "body_shape_entered";
    public const string AREA3D_BODY_SHAPE_EXITED = "body_shape_exited";
    
    public const string RIGIDBODY3D_BODY_ENTERED = "body_entered";
    public const string RIGIDBODY3D_BODY_EXITED = "body_exited";
    public const string RIGIDBODY3D_BODY_SHAPE_ENTERED = "body_shape_entered";
    public const string RIGIDBODY3D_BODY_SHAPE_EXITED = "body_shape_exited";
    public const string RIGIDBODY3D_SLEEPING_STATE_CHANGED = "sleeping_state_changed";

    public const string CONTROL_FOCUS_ENTERED = "focus_entered";
    public const string CONTROL_FOCUS_EXITED = "focus_exited";
    public const string CONTROL_GUI_INPUT = "gui_input";
    public const string CONTROL_MIN_SIZE_CHANGED = "minimum_size_changed";
    public const string CONTROL_MODAL_CLOSED = "modal_closed";
    public const string CONTROL_MOUSE_ENTERED = "mouse_entered";
    public const string CONTROL_MOUSE_EXITED = "mouse_exited";
    public const string CONTROL_RESIZED = "resized";
    public const string CONTROL_SIZE_FLAGS_CHANGED = "size_flags_changed";

    public const string BUTTON_DOWN = "button_down";
    public const string BUTTON_UP = "button_up";
    public const string BUTTON_PRESSED = "pressed";
    public const string BUTTON_TOGGLED = "toggled";

    public const string RANGE_CHANGED = "changed";
    public const string RANGE_VALUE_CHANGED = "value_changed";

    public const string LINEEDIT_CHANGE_REJECTED = "text_change_rejected";
    public const string LINEEDIT_CHANGED = "text_changed";
    public const string LINEEDIT_TEXT_ENTERED = "text_entered";
    
    // Potential extensions: Tree, ItemList
    
    public const string ANIMATION_CHANGED = "animation_changed";
    public const string ANIMATION_FINISHED = "animation_finished";
    public const string ANIMATION_STARTED = "animation_started";

    public const string TWEEN_ALL_COMPLETED = "tween_all_completed";
    public const string TWEEN_COMPLETED = "tween_completed";
    public const string TWEEN_STARTED = "tween_started";
    public const string TWEEN_STEP = "tween_step";

    public const string STTWEEN_FINISHED = "finished";
    public const string STTWEEN_LOOP_FINISHED = "loop_finished";
    public const string STTWEEN_STEP_FINISHED = "step_finished";

    public const string AUDIOSTREAM_FINISHED = "finished";
}
