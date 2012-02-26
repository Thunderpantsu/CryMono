/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// The FlowManager handles registering of Mono flownodes.
//////////////////////////////////////////////////////////////////////////
// 23/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __FLOW_MANAGER__
#define __FLOW_MANAGER__

#include "MonoCommon.h"

#include <IMonoScriptBind.h>
#include <IFlowSystem.h>

struct IMonoArray;
class CFlowNode;

struct SNodeData
{
	SNodeData(CFlowNode *pFlowNode) : pNode(pFlowNode) { ReloadPorts(); };

	void ReloadPorts();

	CFlowNode *pNode;

	static SOutputPortConfig *pOutputs;
	static SInputPortConfig *pInputs;
};

class CEntityFlowManager : public IFlowNodeFactory
{
public:
	CEntityFlowManager() : m_refs(0) {}

	// IFlowNodeFactory
	virtual void AddRef() { ++m_refs; }
	// We want to manually kill this off, since it's used so often.
	virtual void Release() { if( 0 >= --m_refs) delete this; }
	IFlowNodePtr Create( IFlowNode::SActivationInfo *pActInfo );

	virtual void GetMemoryUsage(ICrySizer * s) const
	{ 
		SIZER_SUBCOMPONENT_NAME(s, "CEntityFlowManager");
		s->Add(*this); 
	}
	virtual void Reset() {}
	// ~IFlowNodeFactory

protected:
	int m_refs;
};

class CFlowManager : public IMonoScriptBind, IFlowNodeFactory
{
public:
	CFlowManager();
	~CFlowManager() {}

	typedef std::map<int, SNodeData *> TFlowNodes;

	// IFlowNodeFactory
	virtual void AddRef() override { ++m_refs; }
	// We want to manually kill this off, since it's used so often.
	virtual void Release() override { if( 0 >= --m_refs) delete this; }
	IFlowNodePtr Create( IFlowNode::SActivationInfo *pActInfo ) override;

	virtual void GetMemoryUsage(ICrySizer * s) const override
	{ 
		SIZER_SUBCOMPONENT_NAME(s, "CFlowManager");
		s->Add(*this); 
	}
	virtual void Reset() override;
	// ~IFlowNodeFactory

	static void RegisterFlowNode(CFlowNode *pNode, int scriptId) { m_nodes.insert(TFlowNodes::value_type(scriptId, new SNodeData(pNode))); }
	static void UnregisterFlowNode(int id);

	static SNodeData *GetNodeDataById(int scriptId);
	static CFlowNode *GetNodeById(int scriptId);

	CEntityFlowManager *GetEntityFlowManager() const { return m_pEntityFlowManager; }

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "FlowSystem"; }
	// ~IMonoScriptBind

	static void RegisterNode(mono::string, mono::string, bool);

	static bool IsPortActive(int, int);

	template <class T>
	static void ActivateOutputOnNode(int scriptId, int index, const T &value);
	
	static int GetPortValueInt(int, int);
	static float GetPortValueFloat(int, int);
	static EntityId GetPortValueEntityId(int, int);
	static mono::string GetPortValueString(int, int);
	static bool GetPortValueBool(int, int);
	static mono::object GetPortValueVec3(int, int);

	static void ActivateOutput(int, int);
	static void ActivateOutputInt(int, int, int);
	static void ActivateOutputFloat(int, int, float);
	static void ActivateOutputEntityId(int, int, EntityId);
	static void ActivateOutputString(int, int, mono::string);
	static void ActivateOutputBool(int, int, bool);
	static void ActivateOutputVec3(int, int, Vec3);

	static TFlowNodes m_nodes;

	CEntityFlowManager *m_pEntityFlowManager;

	int m_refs;
};

#endif //__FLOW_MANAGER__