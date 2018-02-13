using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InAudioSystem;

public class StateManager : MonoBehaviourEX<StateManager>
{
	public GlobalDefine.RhythmState m_RhythmState;
	public InMusicGroup[] m_MusicGroups;

	public float GuardsSpeedModifier=1f,GuardsTurnSpeedModifier=1f,PlayerSpeedModifier=1f;
	public float m_pitchValue = 1f;
	public bool GuardsVisionNormal = true;
	

	void Start()
	{
		ChangeState(m_RhythmState);
	}

	public void ChangeState(GlobalDefine.RhythmState RhythmState)
	{
		m_RhythmState = RhythmState;
		switch (RhythmState)
		{
			case GlobalDefine.RhythmState.FastMovingSong:
				InAudio.Music.StopAll();
				InAudio.Music.PlayWithFadeIn(m_MusicGroups[0],0.4f);
				GuardsTurnSpeedModifier = 1f;
				GuardsVisionNormal = true;
				PlayerSpeedModifier = 1f;
				GuardsSpeedModifier = 1f;
				break;
				case GlobalDefine.RhythmState.StealthSong:
					InAudio.Music.StopAll();
					InAudio.Music.PlayWithFadeIn(m_MusicGroups[1],0.4f);
					GuardsTurnSpeedModifier = 0.5f;
					GuardsVisionNormal = false;
					PlayerSpeedModifier = 0.2f;
					GuardsSpeedModifier = 0.6f;
					
					break;
		}
	}

	public void ChangeState(int state)
	{
		//m_RhythmState = (GlobalDefine.RhythmState)state;
		ChangeState((GlobalDefine.RhythmState)state);
	}

	public void ChangePitch(float pitchValue)
	{
		m_pitchValue = pitchValue;
		InAudio.Music.SetPitch(m_MusicGroups[(int)m_RhythmState],pitchValue);
	}
}
