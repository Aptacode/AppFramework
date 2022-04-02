namespace Aptacode.AppFramework.Events;

public abstract record KeyboardEvent(string Key) : UiEvent;

public record KeyDownEvent(string Key) : KeyboardEvent(Key);

public record KeyUpEvent(string Key) : KeyboardEvent(Key);