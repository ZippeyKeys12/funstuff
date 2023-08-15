using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Components.AI.NPC
{
    [BurstCompile]
    public struct NPCData : IComponentData
    {
        // Personality
        public float o, c, e, a, n;

        public float2 oc => new float2(o,c);
        public float3 oce => new float3(o,c,e);
        public float4 ocea => new float4(o,c,e,a);
        public float4 ocen => new float4(o,c,e,n);

        public float3 oca => new float3(o,c,a);
        public float4 ocan => new float4(o,c,a,n);
        
        public float3 ocn => new float3(o,c,n);
        
        public float2 oe => new float2(o,e);
        public float3 oea => new float3(o,e,a);
        public float4 oean => new float4(o,e,a,n);
        
        public float3 oen => new float3(o,e,n);
        
        public float2 oa => new float2(o,a);
        public float3 oan => new float3(o,a,n);
        
        public float2 on => new float2(o,n);
        
        public float2 ce => new float2(c,e);
        public float3 cea => new float3(c, e, a);
        public float4 cean => new float4(c,e,a,n);
        
        public float3 cen => new float3(c,e,n);
        
        public float2 ca => new float2(c,a);
        public float3 can => new float3(c,a,n);
        
        public float2 cn => new float2(c,n);
        
        public float2 ea => new float2(e,a);
        public float3 ean => new float3(e,a,n);
        
        public float2 en => new float2(e,n);
        
        public float2 an => new float2(a,n);

        // ERG, now with enlightenment!
        public ERGEState developmentalState;
        
        public enum ERGEState
        {
            Existence,
            Relatedness,
            Growth,
            Enlightenment
        }
        
        public float existence
            => math.dot(existenceNeeds, .5f);
        
        public float relatedness
            => math.dot(relatednessNeeds, .5f);
        
        public float growth
            => math.dot(growthNeeds, .5f);

        public float enlightenment
            => math.dot(enlightenmentNeeds, .5f);

        // Maslow //
        // Deficiency
        public float physiological, safety;
        public float2 existenceNeeds
            => new float2(physiological, safety);
        
        public float belonging, esteem;
        public float2 relatednessNeeds
            => new float2(belonging, esteem);

        // Growth
        public float cognitive, aesthetic;
        public float2 growthNeeds
            => new float2(cognitive, aesthetic);
        
        public float actualization, transcendence;
        public float2 enlightenmentNeeds
            => new float2(actualization, transcendence);
    }
}